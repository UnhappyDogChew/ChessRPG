using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public class BattleState : State
    {
        bool transforming;
        bool timePaused;

        int timespan;

        Player player;
        GUILayer guiLayer;
        GameObjectLayer gameObjectLayer;
        EffectLayer topEffectLayer;
        Combat combat;
        BattleStage stage;

        ActionMenu actionMenu;
        ActionSelect actionSelect;
        Label instruction;

        Selector selector;
        SelectedEffect selectedEffect;

        FighterObject actingFighterObject;
        Action currentAction;

        const int TRANSFORMING_TIME = 90;
        const int PLAYEROFFSET_Y = 207;
        const int HEROFRONT_Y = 382;
        const int HEROBEHIND_Y = 468;
        const int HERO_GAP_X = 72;

        const int ENEMYFRONT_Y = 238;
        const int ENEMY_GAP_X = 72;
        const int ENEMY_GAP_Y = -84;

        const int MENUSTART_X = 0;
        const int MENUSTART_Y = 468;
        const int SELECTSTART_X = 160;
        const int INSTRUCTION_X = 256;
        const int INSTRUCTION_Y = 560;

        public BattleState(Combat combat) : base()
        {
            this.combat = combat;
        }

        public BattleState(List<Keys> pressedKeys, Combat combat) : base(pressedKeys)
        {
            this.combat = combat;
        }

        public override void Update(GameTime gameTime)
        {
            if (transforming) // Starting
            {
                timespan++;
                if (timespan > TRANSFORMING_TIME)
                {
                    #region Summon Enemy and Fighter objects.
                    foreach (Enemy enemy in combat.enemyList)
                    {
                        EnemyObject enemyObject = new EnemyObject(0, 0, enemy);
                        guiLayer.AddGUI(new EnemyGages(enemy.name + "Gages", null, enemyObject));
                        stage.AddFighterObject(enemyObject);
                    }
                    foreach (Hero hero in player.heros)
                    {
                        HeroObject heroObject = new HeroObject(0, 0, hero);
                        guiLayer.AddGUI(new HeroGages(hero.name + "Gages", null, heroObject));
                        stage.AddFighterObject(heroObject);
                    }
                    SetFighterObjects();
                    #endregion
                    #region Set Selector
                    int i = 0;
                    foreach (ISelectable element in actionMenu.GetChild())
                    {
                        selector.SetItemToMatrix("ActionMenuGroup", element, i++, 0);
                    }
                    selector.SetItemToMatrix("ActionSelectGroup", actionSelect, 0, 0);

                    SetFighterGroupMatrix();
                    #endregion

                    transforming = false;
                    timePaused = false;
                }
                else
                {
                    Global.camera.offsetY += (float)PLAYEROFFSET_Y / TRANSFORMING_TIME;
                }
            }
            else // Battle Started.
            {
                if (timePaused)
                {
                    KeyCheck();
                }
                else
                {
                    //UpdateBattleStage();
                    foreach (FighterObject fighterObject in stage)
                    {
                        if (fighterObject == null)
                            continue;

                        Fighter fighter = fighterObject.fighter;

                        if (fighter.IncreaseAP(60))
                        {
                            actingFighterObject = fighterObject;
                            if (fighter is Hero)
                            {
                                ActionMenuEnable((HeroObject)fighterObject);
                            }
                        }
                    }
                }

            }
            base.Update(gameTime);
        }

        private void UpdateFighterObjectLocations()
        {
            SetFighterGroupMatrix();
            actingFighterObject.fighter.ResetAP();
            selectedEffect?.Finish();
            selectedEffect = null;
            timePaused = false;
        }

        private void RemoveDeadFighters()
        {
            foreach (FighterObject fighterObject in stage)
            {
                if (!fighterObject.fighter.alive)
                {
                    fighterObject.Kill();
                    stage.RemoveFighterObject(fighterObject);
                }
            }
            UpdateFighterObjectLocations();
        }

        public void AddTargetBuffer(FighterObject target)
        {
            currentAction?.Execute(stage, actingFighterObject, target, RemoveDeadFighters);
            actionMenu.Deactivate();
            actionSelect.Deactivate();
            instruction.Deactivate();
            selector.Reset();
        }

        public void SelectTarget(Action action)
        {
            currentAction = action;
            selector.ChangeMatrix("FighterGroup");

            #region MoveAction NullFighterObject creation
            if (action is MoveAction)
            {
                if (actingFighterObject.fighter.state == FighterState.Behind)
                {
                    // Front
                    int heroFrontCount = stage.fighterLists[(int)StageRow.HeroFront].Count;
                    if (heroFrontCount < 5)
                    {
                        int startX;
                        if (heroFrontCount == 0)
                            startX = Global.camera.x + Global.camera.width / 2;
                        else
                            startX = stage.fighterLists[(int)StageRow.HeroFront][heroFrontCount - 1].x + HERO_GAP_X;

                        NullFighterObject nullFighterObject = new NullFighterObject(startX,
                            Global.camera.y + HEROFRONT_Y, FighterType.Hero, FighterState.Front);
                        selector.SetItemToMatrixAtLast("FighterGroup", nullFighterObject, (int)StageRow.HeroFront);
                        stage.AddFighterObject(nullFighterObject);
                    }
                }
                else if (actingFighterObject.fighter.state == FighterState.Front)
                {
                    // Behind
                    int heroBehindCount = stage.fighterLists[(int)StageRow.HeroBehind].Count;
                    if (heroBehindCount < 5)
                    {
                        int startX;
                        if (heroBehindCount == 0)
                            startX = Global.camera.x + Global.camera.width / 2;
                        else
                            startX = stage.fighterLists[(int)StageRow.HeroBehind][heroBehindCount - 1].x + HERO_GAP_X;

                        NullFighterObject nullFighterObject = new NullFighterObject(startX,
                            Global.camera.y + HEROBEHIND_Y, FighterType.Hero, FighterState.Behind);
                        selector.SetItemToMatrixAtLast("FighterGroup", nullFighterObject, (int)StageRow.HeroBehind);
                        stage.AddFighterObject(nullFighterObject);
                    }
                }
            }
            #endregion

            string targetString = action.targetType.ToString().Substring(3);
            FighterState actingFighterState = actingFighterObject.fighter.state;
            switch (targetString)
            {
                case "Enemy": selector.constraint = (int row, int col) => (row == 0 || row == 1); break;
                case "EnemyFront": selector.constraint = (int row, int col) => (row == 1); break;
                case "EnemyBehind": selector.constraint = (int row, int col) => (row == 0); break;
                case "Alley": selector.constraint = (int row, int col) => (row == 2 || row == 3); break;
                case "AlleyFront": selector.constraint = (int row, int col) => (row == 2); break;
                case "AlleyBehind": selector.constraint = (int row, int col) => (row == 3); break;
                case "AlleyOther": selector.constraint =
                    (int row, int col) => {
                        if (selector.GetItem("FighterGroup", row, col).Equals(actingFighterObject))
                            return false;
                        return (row == 2 || row == 3);
                    }; break;
                case "AlleyOtherLine": selector.constraint =
                    (int row, int col) => {
                        if ((row == 2 && actingFighterState == FighterState.Front) ||
                            (row == 3 && actingFighterState == FighterState.Behind))
                            return false;
                        return (row == 2 || row == 3);
                    }; break;
                case "Other": selector.constraint =
                    (int row, int col) => {
                        if (selector.GetItem("FighterGroup", row, col).Equals(actingFighterObject))
                            return false;
                        return true;
                    }; break;
                default: selector.ResetConstraint(); break;
            }
            if (action.targetType.ToString().Substring(0, 3) == "All")
                selector.ChangeSelectMode(SelectMode.All);
            else if (action.targetType.ToString().Substring(0, 3) == "Row")
                selector.ChangeSelectMode(SelectMode.Row);
            else
                selector.ChangeSelectMode(SelectMode.One);

            selector.SelectFirstItem();
            actionSelect.Deactivate();
            actionMenu.Deactivate();
            instruction.Activate();
        }

        public void ActionSelectEnable(string type)
        {
            actionSelect.Activate();
            selector.ChangeMatrix("ActionSelectGroup");
            selector.ResetConstraint();
            selector.ChangeSelectMode(SelectMode.One);
            selector.SelectFirstItem();
            switch (type)
            {
                case "Attack": actionSelect.SetActions(((Hero)actingFighterObject.fighter).attacks); break;
                case "Skill": actionSelect.SetActions(((Hero)actingFighterObject.fighter).skills); break;
                case "Move": actionSelect.SetActions(new Action[] { new MoveAction() }); break;
            }
        }

        public void ActionMenuEnable(HeroObject heroObject)
        {
            SetSelectedEffect(heroObject);
            actionMenu.Activate();
            selector.ChangeMatrix("ActionMenuGroup");
            selector.ResetConstraint();
            selector.ChangeSelectMode(SelectMode.One);
            selector.SelectFirstItem();
            timePaused = true;
        }
        /// <summary>
        /// Check Combat's Enemy list and summon them in the <see cref=" BattleStage"/>.
        /// </summary>
        private void SummonEnemies()
        {
            int frontStartX = (Global.camera.width - (combat.frontCount - 1) * ENEMY_GAP_X) / 2;
            int behindStartX = (Global.camera.width - (combat.behindCount - 1) * ENEMY_GAP_X) / 2;
            int enemyFrontCount = 0;
            int enemyBehindCount = 0;
            foreach (Enemy enemy in combat.enemyList)
            {
                EnemyObject fighterObject;
                if (enemy.state == FighterState.Front)
                {
                    fighterObject =
                        new EnemyObject(Global.camera.x + frontStartX + ENEMY_GAP_X * enemyFrontCount,
                            Global.camera.y + ENEMYFRONT_Y, enemy);
                    guiLayer.AddGUI(new EnemyGages(fighterObject.fighter.name + "Gage", null, fighterObject));
                    stage.AddFighterObject(fighterObject);
                    enemyFrontCount++;
                }
                else if (enemy.state == FighterState.Behind)
                {
                    fighterObject =
                        new EnemyObject(Global.camera.x + behindStartX + ENEMY_GAP_X * enemyBehindCount,
                            Global.camera.y + ENEMYFRONT_Y + ENEMY_GAP_Y, enemy);
                    guiLayer.AddGUI(new EnemyGages(fighterObject.fighter.name + "Gage", null, fighterObject));
                    stage.AddFighterObject(fighterObject);
                    enemyBehindCount++;
                }
            }
        }
        /// <summary>
        /// Check Player's hero list and summon them in the <see cref=" BattleStage"/>.
        /// </summary>
        private void SummonHeros()
        {
            int frontStartX = (Global.camera.width - (player.HeroFrontAmount - 1) * HERO_GAP_X) / 2;
            int behindStartX = (Global.camera.width - (player.HeroBehindAmount - 1) * HERO_GAP_X) / 2;
            int heroFrontCount = 0;
            int heroBehindCount = 0;
            foreach (Hero hero in player.heros)
            {
                HeroObject fighterObject;
                if (hero.state == FighterState.Front)
                {
                    fighterObject =
                        new HeroObject(Global.camera.x + frontStartX + ENEMY_GAP_X * heroFrontCount,
                            Global.camera.y + HEROFRONT_Y, hero);
                    guiLayer.AddGUI(new HeroGages(fighterObject.fighter.name + "Gage", null, fighterObject));
                    stage.AddFighterObject(fighterObject);
                    heroFrontCount++;
                }
                else if (hero.state == FighterState.Behind)
                {
                    fighterObject =
                        new HeroObject(Global.camera.x + behindStartX + HERO_GAP_X * heroBehindCount,
                            Global.camera.y + HEROBEHIND_Y, hero);
                    guiLayer.AddGUI(new HeroGages(fighterObject.fighter.name + "Gage", null, fighterObject));
                    stage.AddFighterObject(fighterObject);
                    heroBehindCount++;
                }
            }
        }

        private void SetFighterObjects()
        {
            int enemyFrontStartX = (Global.camera.width - (stage.fighterLists[1].Count - 1) * ENEMY_GAP_X) / 2;
            int enemyBehindStartX = (Global.camera.width - (stage.fighterLists[0].Count - 1) * ENEMY_GAP_X) / 2;
            int heroFrontStartX = (Global.camera.width - (stage.fighterLists[2].Count - 1) * HERO_GAP_X) / 2;
            int heroBehindStartX = (Global.camera.width - (stage.fighterLists[3].Count - 1) * HERO_GAP_X) / 2;
            int heroFrontCount = 0;
            int heroBehindCount = 0;
            int enemyFrontCount = 0;
            int enemyBehindCount = 0;

            foreach (FighterObject fighterObject in stage)
            {
                if (fighterObject is EnemyObject || 
                    (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Enemy))
                {
                    if (fighterObject.fighter.state == FighterState.Behind)
                    {
                        fighterObject.MoveLocation(new Point(Global.camera.x + enemyBehindStartX + ENEMY_GAP_X * enemyBehindCount,
                            Global.camera.y + ENEMYFRONT_Y + ENEMY_GAP_Y));
                        enemyBehindCount++;
                    }
                    else
                    {
                        fighterObject.MoveLocation(new Point(Global.camera.x + enemyFrontStartX + ENEMY_GAP_X * enemyFrontCount,
                            Global.camera.y + ENEMYFRONT_Y));
                        enemyFrontCount++;
                    }
                }
                else // Set HeroObjects.
                {
                    if (fighterObject.fighter.state == FighterState.Front)
                    {
                        fighterObject.MoveLocation(new Point(Global.camera.x + heroFrontStartX + ENEMY_GAP_X * heroFrontCount,
                            Global.camera.y + HEROFRONT_Y));
                        heroFrontCount++;
                    }
                    else
                    {
                        fighterObject.MoveLocation(new Point(Global.camera.x + heroBehindStartX + HERO_GAP_X * heroBehindCount,
                            Global.camera.y + HEROBEHIND_Y));
                        heroBehindCount++;
                    }
                }
            }
        }

        private void SetFighterGroupMatrix()
        {
            selector.ClearMatrix("FighterGroup");
            int col = 0;
            int row = 0;
            foreach (FighterObject fighterObject in stage)
            {
                if (fighterObject == null)
                    continue;
                if ((row == 0 && fighterObject is EnemyObject && ((EnemyObject)fighterObject).enemy.state == FighterState.Front) ||
                    (row == 1 && fighterObject is HeroObject) ||
                    (row == 2 && fighterObject is HeroObject && ((HeroObject)fighterObject).hero.state == FighterState.Behind))
                {
                    col = 0; row++;
                }
                selector.SetItemToMatrix("FighterGroup", fighterObject, row, col++);
            }
        }

        private void UpdateBattleStage()
        {
            List<FighterObject> fighterObjects = new List<FighterObject>();
            foreach (FighterObject fighterObject in stage)
            {
                fighterObjects.Add(fighterObject);
            }
        }

        private void Rewind()
        {
            if (selector.matrixName == "FighterGroup")
            {
                actionSelect.Activate();
                actionMenu.Activate();
                instruction.Deactivate();
                RemoveNullFighterObjects();
                selector.ResetConstraint();
                selector.ChangeSelectMode(SelectMode.One);
            }
            if (selector.matrixName == "ActionSelectGroup")
            {
                actionSelect.Deactivate();
                selector.ResetConstraint();
                selector.ChangeSelectMode(SelectMode.One);
            }
            selector.Rewind();
        }

        private void SetSelectedEffect(FighterObject fighterObject)
        {
            if (fighterObject == null)
                return;
            if (selectedEffect == null)
            {
                selectedEffect = new SelectedEffect(fighterObject.x - Global.camera.x, 
                    fighterObject.y - (int)fighterObject.fighter.sprite.origin.Y - Global.camera.y - 30, 10);
                topEffectLayer.elements.Add(selectedEffect);
            }
            else
            {
                selectedEffect.x = fighterObject.x - Global.camera.x;
                selectedEffect.y = fighterObject.y - (int)fighterObject.fighter.sprite.origin.Y - Global.camera.y - 30;
            }
        }

        private void RemoveNullFighterObjects()
        {
            for (int col = 0; col < Global.Properties.FIGHTER_IN_ROW; col++)
            {
                if (selector.GetItem("FighterGroup", 2, col) is NullFighterObject)
                    selector.SetItemToMatrix("FighterGroup", null, 2, col);
                if (selector.GetItem("FighterGroup", 3, col) is NullFighterObject)
                    selector.SetItemToMatrix("FighterGroup", null, 3, col);
            }
            stage.fighterLists[(int)StageRow.HeroFront].RemoveAll((obj) => (obj is NullFighterObject));
            stage.fighterLists[(int)StageRow.HeroBehind].RemoveAll((obj) => (obj is NullFighterObject));
        }

        protected override void Prepare()
        {
            timespan = 0;
            transforming = true;
            timePaused = true;
            player = Global.world.GetPlayer();
            guiLayer = (GUILayer)Global.world.GetLayer("GUILayer");
            gameObjectLayer = (GameObjectLayer)Global.world.GetLayer("GameObjectLayer");
            topEffectLayer = (EffectLayer)Global.world.GetLayer("TopEffectLayer");

            player.Look(Direction.Up);
            player.Stand();

            guiLayer.RemoveGUI("TeamManagement");
            actionMenu = new ActionMenu("ActionMenu", null, MENUSTART_X, MENUSTART_Y);
            actionMenu.Deactivate();
            guiLayer.AddGUI(actionMenu);
            actionSelect = new ActionSelect("ActionSelect", null, SELECTSTART_X, MENUSTART_Y, null);
            actionSelect.Deactivate();
            guiLayer.AddGUI(actionSelect);
            instruction = new Label("Instruction", null, INSTRUCTION_X, INSTRUCTION_Y, "Select Target", Color.White,
                512, Global.content.Load<SpriteFont>("neodgm24"), AlignType.Top, AlignType.Center);
            instruction.Deactivate();
            guiLayer.AddGUI(instruction);

            stage = new BattleStage();
            selector = new Selector(mirrorMode: true);
            selector.CreateNewMatrix("ActionMenuGroup", 1, 5);
            selector.CreateNewMatrix("ActionSelectGroup", 1, 1);
            selector.CreateNewMatrix("FighterGroup", Global.Properties.FIGHTER_IN_ROW, 4); // Two for NullFighterObject.

            // Key setting
            keyActions.Add(new KeyAction(Keys.W, pressed: () => { selector.SelectNext(Direction.Up); }));
            keyActions.Add(new KeyAction(Keys.S, pressed: () => { selector.SelectNext(Direction.Down); }));
            keyActions.Add(new KeyAction(Keys.A, pressed: () => 
                {
                    if (selector.matrixName == "ActionSelectGroup")
                        actionSelect.Next(Direction.Left);
                    else
                        selector.SelectNext(Direction.Left);
                }));
            keyActions.Add(new KeyAction(Keys.D, pressed: () => 
                {
                    if (selector.matrixName == "ActionSelectGroup")
                        actionSelect.Next(Direction.Right);
                    else
                        selector.SelectNext(Direction.Right); 
                }));
            keyActions.Add(new KeyAction(Keys.E, pressed: () => { selector.SelectAction(); }));
            keyActions.Add(new KeyAction(Keys.Q, pressed: () => { Rewind(); }));


            base.Prepare();
        }
    }
}
