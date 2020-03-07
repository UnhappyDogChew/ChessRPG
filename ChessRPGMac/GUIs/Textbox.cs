using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Textbox : GUIGroup
    {
        List<string> textList;
        List<DynamicWord> wordBuffer;
        Sprite background;
        SpriteFont font;
        Texture2D nextSign;
        Texture2D selectSign;

        int interval;
        Color fontColor;

        int index;
        int characterIndex;
        int line;
        int lineWidth;

        int timespan; // Start from 1.
        float yScale;

        bool isEnd;
        bool wordProcessing;
        bool wait;
        bool starting;
        bool shown;
        bool selecting;

        const int START_X = 16;
        const int START_Y = -68;
        const int WIDTH = 480;

        public Textbox(string name, GUIComponent parent, int rx, int ry, List<string> textList, SpriteFont font, int interval = 3) : base(name, parent, rx, ry)
        {
            wordBuffer = new List<DynamicWord>();
            this.textList = textList;
            background = Global.spriteBox.Pick("Textbox");

            nextSign = Global.content.Load<Texture2D>("TextboxNext");
            selectSign = Global.content.Load<Texture2D>("TextboxSelect");

            index = 0;
            characterIndex = 0;
            line = 0;
            lineWidth = 0;
            fontColor = Color.Black;
            isEnd = false;
            starting = true;
            yScale = 0.1f;
            shown = false;

            this.interval = interval;
            timespan = interval - 1;
            this.font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            shown = true;
            background.Draw(gameTime, spriteBatch, x, y, Color.White, yScale:yScale);
            foreach (DynamicWord word in wordBuffer)
            {
                word.Draw(gameTime, spriteBatch);
            }
            if (wait)
                spriteBatch.Draw(nextSign, new Vector2(x + 472, y + 60), Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (!shown)
            {

            }
            else if (starting)
            {
                yScale += 0.1f;
                if (yScale >= 1.0f)
                {
                    yScale = 1.0f;
                    starting = false;
                }
            }
            else if (wait)
            {

            }
            else if (isEnd)
            {
                yScale -= 0.1f;
                if (yScale <= 0.0f)
                {
                    yScale = 0.0f;
                    Finished = true;
                }
            }
            else
            {
                timespan++;
                if (timespan >= interval) // Ticking
                {
                    bool skip = false;
                    timespan = 1; // Resets timespan.
                    while (!wordProcessing && characterIndex < textList[index].Length)
                    {
                        string characterBuffer = "";
                        characterBuffer += GetCharacter();

                        if (characterBuffer == " ") // White space
                        {
                            AddWordBuffer(characterBuffer);
                            characterIndex++;
                            wordProcessing = true;
                        }
                        else if (characterBuffer == "<") // Tag
                        {
                            characterIndex++;
                            skip = true;
                            switch (GetCharacter())
                            {
                                case 'n': characterIndex += 2; NextLine(); break; // Next line.
                                case 'w': characterIndex += 2; wait = true; break; // Wait.
                                case 'r': characterIndex += 2; characterBuffer = ""; // Rests for a while.
                                    while (GetCharacter() != '>')
                                    {
                                        characterBuffer += GetCharacter();
                                        characterIndex++;
                                    }
                                    timespan -= int.Parse(characterBuffer);
                                    characterIndex++; break;
                                case 'f':                                           // Sets interval.
                                    characterIndex += 2; characterBuffer = "";
                                    while (GetCharacter() != '>')
                                    {
                                        characterBuffer += GetCharacter();
                                        characterIndex++;
                                    }
                                    interval = int.Parse(characterBuffer);
                                    characterIndex++; break;
                                case 'c':                                           // Sets color.
                                    characterIndex += 2; characterBuffer = "";
                                    while (GetCharacter() != '>')
                                    {
                                        characterBuffer += GetCharacter();
                                        characterIndex++;
                                    }
                                    fontColor = Toolbox.ParseColor(characterBuffer);
                                    characterIndex++; break;
                            }
                        }
                        else // Normal word
                        {
                            characterIndex++;
                            if (characterIndex < textList[index].Length)
                            {
                                while (GetCharacter() != ' ' &&
                                    GetCharacter() != '<')
                                {
                                    characterBuffer += GetCharacter();
                                    characterIndex++;
                                    if (characterIndex >= textList[index].Length)
                                        break;
                                }
                            }
                            if (lineWidth + font.MeasureString(characterBuffer).X >= WIDTH)
                            {
                                NextLine();
                            }
                            AddWordBuffer(characterBuffer);
                            wordProcessing = true;
                        }

                    }
                    if (wordProcessing && !skip)
                    {
                        if (wordBuffer[wordBuffer.Count - 1].Process())
                            wordProcessing = false;
                    }
                    if (characterIndex >= textList[index].Length && !wordProcessing)
                    {
                        // Increase index resets variables.
                        index++;
                        if (!wait)
                        {
                            Reset();
                        }
                        if (index >= textList.Count)
                        {
                            isEnd = true;
                        }
                    }
                }
            }

            // Update words.
            foreach (DynamicWord word in wordBuffer)
            {
                word.Update(gameTime);
            }

        }

        private void AddWordBuffer(string characterBuffer)
        {
            wordBuffer.Add(new DynamicWord(font, characterBuffer, x + START_X + lineWidth,
                            y + START_Y + line * font.LineSpacing, fontColor));
            lineWidth += (int)font.MeasureString(characterBuffer).X;
        }

        private void NextLine()
        {
            lineWidth = 0;
            line++;
        }

        private void Reset()
        {
            wordBuffer.Clear();
            characterIndex = 0;
            line = 0;
            lineWidth = 0;
        }

        private char GetCharacter()
        {
            return textList[index][characterIndex];
        }

        public bool IsWaiting()
        {
            return wait;
        }

        /// <summary>
        /// Process textbox. returns -1 if dialog is ended, 0 if it's not.
        /// </summary>
        /// <returns>The process.</returns>
        public int Process()
        {
            wait = false;
            Reset();
            if (isEnd)
                return -1;
            else
                return 0;
        }
    }

    public class TextboxAnswer : GUIItem
    {
        string answerString;
        SpriteFont font;

        public TextboxAnswer(string name, GUIComponent parent, int x, int y, string answerString, SpriteFont font) : base(name, parent, x, y)
        {
            this.answerString = answerString;
            this.font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
