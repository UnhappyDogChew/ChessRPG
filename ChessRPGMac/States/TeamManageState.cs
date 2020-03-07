using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class TeamManageState : State
    {
        TeamManagement teamManagement;
        Player player;

        bool dragging;
        IDragable dragGUI;
        DragSocket selectedSocket;
        EffectSprite dragEffect;
        EffectTexture socketEffect = null;
        EffectLayer topEffectLayer;
        GUILayer guiLayer;

        SelectedEffect selectedEffect;

        DragSocketGroup heroFrontGroup;
        DragSocketGroup heroBehindGroup;
        DragSocketGroup heroStoredGroup;

        Selector selector;

        public TeamManageState() : base() { }

        public TeamManageState(List<Keys>pressedKeys) : base(pressedKeys) { }

        public override void Update(GameTime gameTime)
        {
            KeyCheck();

            #region Set hero sockets.
            for (int i = 0; i < Player.HEROFRONT_MAX; i++)
            {
                DragSocket socket = heroFrontGroup.GetSocket(i);
                if (socket == null)
                {
                    selector.SetItemToMatrix("HeroSockets", null, 0, i);
                    continue;
                }
                selector.SetItemToMatrix("HeroSockets", (ISelectable)socket, 0, i);
            }
            for (int i = 0; i < Player.HEROBEHIND_MAX; i++)
            {
                DragSocket socket = heroBehindGroup.GetSocket(i);
                if (socket == null)
                {
                    selector.SetItemToMatrix("HeroSockets", null, 1, i);
                    continue;
                }
                selector.SetItemToMatrix("HeroSockets", (ISelectable)socket, 1, i);
            }
            for (int i = 0; i < Player.HEROSTORED_MAX; i++)
            {
                DragSocket socket = heroStoredGroup.GetSocket(i);
                if (socket == null)
                {
                    selector.SetItemToMatrix("HeroSockets", null, 2, i);
                    continue;
                }
                selector.SetItemToMatrix("HeroSockets", (ISelectable)socket, 2, i);
            }
            #endregion


            #region Drag
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (dragging)
                {
                    dragEffect.x = mouse.X;
                    dragEffect.y = mouse.Y;

                    // Socket effect.
                    bool selected = false;
                    foreach (GUIComponent element in guiLayer.MainGroup)
                    {
                        if (element is DragSocket)
                        {
                            if (((DragSocket)element).IsInside(mouse.X, mouse.Y) && ((DragSocket)element).MatchKey(dragGUI))
                            {
                                selected = true;
                                if (element != selectedSocket)
                                {
                                    selectedSocket = (DragSocket)element;
                                    if (socketEffect is Effect)
                                        socketEffect.Finish();
                                    socketEffect = new EffectTexture(selectedSocket.emphasis, selectedSocket.x, selectedSocket.y, -1);
                                    topEffectLayer.elements.Add(socketEffect);
                                }
                            }
                        }
                    }
                    if (!selected)
                    {
                        if (socketEffect is Effect)
                            socketEffect.Finish();
                        selectedSocket = null;
                    }
                }
                else
                {
                    foreach (GUIComponent element in guiLayer.MainGroup)
                    {
                        if (element is IDragable)
                        {
                            IDragable dragable = (IDragable)element;
                            if (dragable.IsInside(mouse.X, mouse.Y))
                            {
                                dragGUI = dragable;
                                Texture2D dragTexture = dragable.GetTexture();
                                Vector2 dragOrigin = dragable.GetOrigin(mouse.X, mouse.Y);
                                Sprite dragSprite = new Sprite(dragTexture, dragOrigin);
                                dragEffect = new EffectSprite(dragSprite, mouse.X, mouse.Y);
                                topEffectLayer.elements.Add(dragEffect);
                                dragging = true;
                                break;
                            }
                        }

                    }
                }

            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                if (dragging)
                {
                    foreach (GUIComponent element in guiLayer.MainGroup)
                    {
                        if (element is DragSocket)
                        {
                            DragSocket newSocket = (DragSocket)element;
                            if (newSocket.IsInside(mouse.X, mouse.Y))
                            {
                                if (newSocket.SetContent(dragGUI))
                                    break;
                            }
                        }
                    }
                    dragging = false;
                    dragEffect?.Finish();
                    socketEffect?.Finish();
                }
            }
            #endregion

            base.Update(gameTime);
        }

        public void SelectHero(HeroSummarySocket socket)
        {
            teamManagement.SelectHero(((HeroSummary)socket.content).hero);
            if (selectedEffect == null)
            {
                selectedEffect = new SelectedEffect(socket.x + HeroSummary.WIDTH / 2, socket.y - 2, 10);
                topEffectLayer.elements.Add(selectedEffect);
            }
            else
            {
                selectedEffect.x = socket.x + HeroSummary.WIDTH / 2;
                selectedEffect.y = socket.y - 2;
            }
        }

        public void DeselectHero()
        {
            teamManagement.DeselectHero();
            selectedEffect?.Finish();
            selectedEffect = null;
        }

        protected override void Prepare()
        {
            player = Global.world.GetPlayer();
            player.Stand();

            guiLayer = (GUILayer)Global.world.GetLayer("GUILayer");
            topEffectLayer = (EffectLayer)Global.world.GetLayer("TopEffectLayer");

            teamManagement = (TeamManagement)((GUILayer)Global.world.GetLayer("GUILayer")).GetGUI<TeamManagement>();
            teamManagement.full = true;

            selector = new Selector(mirrorMode: true);
            selector.CreateNewMatrix("HeroSockets", Player.HEROFRONT_MAX, 3);
            selector.ChangeMatrix("HeroSockets");

            heroFrontGroup = (DragSocketGroup)teamManagement.FindComponent("HeroFrontGroup");
            heroBehindGroup = (DragSocketGroup)teamManagement.FindComponent("HeroBehindGroup");
            heroStoredGroup = (DragSocketGroup)teamManagement.FindComponent("HeroStoredGroup");

            keyActions.Add(new KeyAction(Keys.Enter, 
                pressed: () => { ChangeState(new ExploreState(pressedKeys)); teamManagement.full = false; }));
            keyActions.Add(new KeyAction(Keys.W,
                pressed: () => { selector.SelectNext(Direction.Up); }));
            keyActions.Add(new KeyAction(Keys.S,
                pressed: () => { selector.SelectNext(Direction.Down); }));
            keyActions.Add(new KeyAction(Keys.A,
                pressed: () => { selector.SelectNext(Direction.Left); }));
            keyActions.Add(new KeyAction(Keys.D,
                pressed: () => { selector.SelectNext(Direction.Right); }));
            keyActions.Add(new KeyAction(Keys.E,
                pressed: () => { selector.SelectAction(); }));
            keyActions.Add(new KeyAction(Keys.Q,
                pressed: () => { selector.Rewind(); DeselectHero(); }));

            dragging = false;

            base.Prepare();
        }

        protected override void Finish()
        {
            // Finish effects.
            dragEffect?.Finish();
            socketEffect?.Finish();
            selectedEffect?.Finish();
            selector.Finish();
            base.Finish();
        }
    }
}
