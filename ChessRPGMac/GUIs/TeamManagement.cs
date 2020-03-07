using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class TeamManagement : GUIGroup
    {
        Texture2D background;
        Texture2D frame;
        SpriteFont font16;
        Player player;

        public override int y { get { return (full) ? parent.y + relativeY : parent.y + relativeY + DOWN_HEIGHT; } }

        public bool full;

        Hero currentHero;

        int frontCount;
        int behindCount;
        int storedCount;

        DragSocketGroup frontGroup;
        DragSocketGroup behindGroup;
        DragSocketGroup storedGroup;
        DragSocketGroup itemGroup;

        SkillIcon[] skills;

        const int DOWN_HEIGHT = 238;
        const int FRONTSTART_X = 48;
        const int FRONTSTART_Y = 72;
        const int BEHINDSTART_Y = 124;
        const int TEAM_GAP = 100;
        const int STORESTART_X = 6;
        const int STORESTART_Y = 212;
        const int STORE_GAP = 102;
        const int STATUSSTART_X1 = 15;
        const int STATUSSTART_X2 = 64;
        const int STATUSSTART_X3 = 122;
        const int STATUSSTART_X4 = 155;
        const int STATUSSTART_Y = 300;
        const int STATUSGAP_Y = 14;
        const int ITEMSTART_X = 260;
        const int ITEMSTART_Y = 302;
        const int ITEM_GAP = 36;
        const int SKILLSTART_X = 15;
        const int SKILLSTART_Y = 370;
        const int SKILL_GAP = 48;


        public TeamManagement(string name, GUIComponent parent, int rx, int ry) : base(name, parent, rx, ry)
        {
            background = Global.content.Load<Texture2D>("TeamManagementBackground");
            frame = Global.content.Load<Texture2D>("TeamManagementFrame");
            font16 = Global.content.Load<SpriteFont>("neodgm12");
            full = false;
            player = Global.world.GetPlayer();

            // Prepare hero socket
            frontGroup = new DragSocketGroup("HeroFrontGroup", this, FRONTSTART_X, FRONTSTART_Y, Player.HEROFRONT_MAX);
            behindGroup = new DragSocketGroup("HeroBehindGroup", this, FRONTSTART_X, BEHINDSTART_Y, Player.HEROBEHIND_MAX);
            storedGroup = new DragSocketGroup("HeroStoredGroup", this, STORESTART_X, STORESTART_Y, Player.HEROSTORED_MAX);
            components.Add(frontGroup);
            components.Add(behindGroup);
            components.Add(storedGroup);

            for (int i = 0; i < Player.HEROSTORED_MAX; i++)
            {
                storedGroup.AddSocket(i, new HeroSummarySocket("HeroStored", this, STORE_GAP * i, 0, 
                    new string[] { "HeroFront", "HeroBehind" }));
            }

            // Prepare item socket
            itemGroup = new DragSocketGroup("ItemGroup", this, ITEMSTART_X, ITEMSTART_Y, Player.ITEM_MAX);
            components.Add(itemGroup);

            for (int row = 0; row < Player.ITEM_HEIGHT; row++)
            {
                for (int col = 0; col < Player.ITEM_WIDTH; col++)
                {
                    itemGroup.AddSocket(col + row * Player.ITEM_WIDTH, new ItemSocket("ItemSocket", this,
                        ITEM_GAP * col, ITEM_GAP * row, col, row));
                }
            }

            // Prepare skill icons
            skills = new SkillIcon[5];
            for (int i = 0; i < Player.SKILL_MAX; i++)
            {
                skills[i] = new SkillIcon("Skill" + i, this, SKILLSTART_X + SKILL_GAP * i, SKILLSTART_Y);
                components.Add(skills[i]);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(background, new Vector2(x, y), Color.White);
            // Draw contents
            foreach (GUIComponent component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            if (full)
            {
                // Draw status
                spriteBatch.DrawString(font16, "HP:", 
                    new Vector2(x + STATUSSTART_X1, y + STATUSSTART_Y), Color.White);
                spriteBatch.DrawString(font16, "SPEED:", 
                    new Vector2(x + STATUSSTART_X1, y + STATUSSTART_Y + STATUSGAP_Y), Color.White);
                spriteBatch.DrawString(font16, "MANA:", 
                    new Vector2(x + STATUSSTART_X1, y + STATUSSTART_Y + STATUSGAP_Y * 2), Color.White);
                spriteBatch.DrawString(font16, "STR:", 
                    new Vector2(x + STATUSSTART_X3, y + STATUSSTART_Y), Color.White);
                spriteBatch.DrawString(font16, "INT:", 
                    new Vector2(x + STATUSSTART_X3, y + STATUSSTART_Y + STATUSGAP_Y), Color.White);
                spriteBatch.DrawString(font16, "DEF:", 
                    new Vector2(x + STATUSSTART_X3, y + STATUSSTART_Y + STATUSGAP_Y * 2), Color.White);

                if (currentHero != null)
                {
                    spriteBatch.DrawString(font16, currentHero.maxHp.ToString(), 
                        new Vector2(x + STATUSSTART_X2, y + STATUSSTART_Y), Color.White);
                    spriteBatch.DrawString(font16, currentHero.speed.ToString(), 
                        new Vector2(x + STATUSSTART_X2, y + STATUSSTART_Y + STATUSGAP_Y), Color.White);
                    spriteBatch.DrawString(font16, currentHero.mana.ToString(), 
                        new Vector2(x + STATUSSTART_X2, y + STATUSSTART_Y + STATUSGAP_Y * 2), Color.White);
                    spriteBatch.DrawString(font16, currentHero.strength.ToString(), 
                        new Vector2(x + STATUSSTART_X4, y + STATUSSTART_Y), Color.White);
                    spriteBatch.DrawString(font16, currentHero.intelligence.ToString(), 
                        new Vector2(x + STATUSSTART_X4, y + STATUSSTART_Y + STATUSGAP_Y), Color.White);
                    spriteBatch.DrawString(font16, currentHero.defense.ToString(), 
                        new Vector2(x + STATUSSTART_X4, y + STATUSSTART_Y + STATUSGAP_Y * 2), Color.White);
                    for (int i = 0; i < Player.SKILL_MAX; i++)
                    {
                        skills[i].skill = currentHero.skills[i];
                    }
                }

                spriteBatch.Draw(frame, new Vector2(x, y), Color.White);
            }
            else
            {
            }
            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            SetHeros();
            SetItems();
            base.Update(gameTime);
        }

        public void SelectHero(Hero hero) { currentHero = hero; }

        public void DeselectHero() 
        { 
            currentHero = null; 
            foreach (SkillIcon icon in skills)
            {
                icon.skill = null;
            }
        }

        private void SetHeros()
        {
            // Set sockets
            for (int i = 0; i < Player.HEROFRONT_MAX; i++)
            {
                if (i < player.HeroFrontAmount + 1)
                    frontGroup.AddSocket(i, new HeroSummarySocket("HeroFront", this, TEAM_GAP * i, 0,
                        new string[] { "HeroBehind", "HeroStored" }));
                else
                    frontGroup.RemoveSocket(i);
            }
            for (int i = 0; i < Player.HEROBEHIND_MAX; i++)
            {
                if (i < player.HeroBehindAmount + 1)
                    behindGroup.AddSocket(i, new HeroSummarySocket("HeroBehind", this, TEAM_GAP * i, 0,
                        new string[] { "HeroFront", "HeroStored" }));
                else
                    behindGroup.RemoveSocket(i);
            }

            // Fill HeroSummarys
            List<HeroSummary> currentList = new List<HeroSummary>();
            foreach (GUIComponent component in frontGroup)
            {
                if (component is HeroSummary)
                    currentList.Add((HeroSummary)component);
            }
            foreach (GUIComponent component in behindGroup)
            {
                if (component is HeroSummary)
                    currentList.Add((HeroSummary)component);
            }
            foreach (GUIComponent component in storedGroup)
            {
                if (component is HeroSummary)
                    currentList.Add((HeroSummary)component);
            }

            frontCount = 0; behindCount = 0; storedCount = 0;
            foreach (Hero hero in player.heros)
            {
                bool exist = false;
                foreach (HeroSummary summary in currentList)
                {
                    if (summary.hero == hero) // If herosummary is exist, move it on right place.
                    {
                        exist = true;
                        if (summary.parent.name != "Hero" + hero.state.ToString())
                        {
                            ChangeSummaryGroup(summary, hero.state);
                        }
                        switch (hero.state)
                        {
                            case FighterState.Front: frontCount++; break;
                            case FighterState.Behind: behindCount++; break;
                            case FighterState.Stored: storedCount++; break;
                            default: throw new Exception("Invalid Hero State.");
                        }
                    }
                }
                if (!exist) // If doesn't exist, Create one.
                {
                    switch (hero.state)
                    {
                        case FighterState.Front: 
                            frontGroup.SetContent(frontCount, 
                                new HeroSummary(hero.name + "Summary", null, 0, 0, hero), true);
                            frontCount++; break;
                        case FighterState.Behind:
                            behindGroup.SetContent(behindCount,
                                new HeroSummary(hero.name + "Summary", null, 0, 0, hero), true);
                            behindCount++; break;
                        case FighterState.Stored:
                            storedGroup.SetContent(storedCount,
                                new HeroSummary(hero.name + "Summary", null, 0, 0, hero), true);
                            storedCount++; break;
                    }
                }
            }

        }

        private void SetItems()
        {
            for (int row = 0; row < Player.ITEM_HEIGHT; row++)
            {
                for (int col = 0; col < Player.ITEM_WIDTH; col++)
                {
                    Item item = player.items[row, col];
                    DragSocket socket = itemGroup.GetSocket(col + row * Player.ITEM_WIDTH);
                    if (item != null && socket.content == null)
                    {
                        socket.SetContent(new ItemIcon(item.name, socket, 0, 0, item));
                    }
                    if (item == null && socket.content != null)
                    {
                        socket.ResetContent();
                    }
                }
            }
        }

        /// <summary>
        /// Changes the summary group. Index is state Count.
        /// </summary>
        /// <returns><c>true</c>, if summary group was changed, <c>false</c> otherwise.</returns>
        /// <param name="summary">Summary.</param>
        /// <param name="state">State.</param>
        private bool ChangeSummaryGroup(HeroSummary summary, FighterState state) 
        {
            bool result;
            switch (state)
            {
                case FighterState.Front: result = frontGroup.SetContent(frontCount, summary); break;
                case FighterState.Behind: result = behindGroup.SetContent(behindCount, summary); break;
                case FighterState.Stored: result = storedGroup.SetContent(storedCount, summary); break;
                default: throw new Exception("Invalid Hero State.");
            }
            return result;
        }
    }
}
