using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ChessRPGMac
{
    public class InteractionState : State
    {
        Interaction interaction;
        GUILayer guiLayer;

        public InteractionState(List<Keys> pressedKeys, Interaction interaction) : base(pressedKeys)
        {
            this.interaction = interaction;
            guiLayer = (GUILayer)Global.world.GetLayer("GUILayer");
            guiLayer.AddGUI(interaction.InteractionTextbox);
        }

        public override void Update(GameTime gameTime)
        {
            KeyCheck();

            if (interaction.IsEnd)
                ChangeState(new ExploreState(pressedKeys));

            base.Update(gameTime);
        }

        protected override void Prepare()
        {
            keyActions.Add(new KeyAction(Keys.E, pressed: () =>
            {
                if (interaction.InteractionTextbox.IsWaiting())
                {
                    int value = interaction.InteractionTextbox.Process();
                    switch (value)
                    {
                        case -1: interaction.Finish(); break;
                        case 0: break;
                        default: interaction.ChooseAnswer(value); break;
                    }
                }
            }));

            base.Prepare();
        }
    }
}
