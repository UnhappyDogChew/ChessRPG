using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    /// <summary>
    /// World is collection of layers. It manipulates layers, draw layers, and find out specific object in layers.
    /// </summary>
    public class World
    {
        List<Layer> layers;

        /// <summary>
        /// List of GameObjectLayers. Readonly.
        /// This list is automatically managed by <see cref="World"/> instance.
        /// </summary>
        public readonly List<GameObjectLayer> GameObjectLayers;
        public readonly List<GUILayer> GUILayers;
        public string Name { get; private set; }

        public World(string name)
        {
            Name = name;
            layers = new List<Layer>();
            GameObjectLayers = new List<GameObjectLayer>();
            GUILayers = new List<GUILayer>();
        }
        /// <summary>
        /// Adds the layers to the world.
        /// </summary>
        /// <param name="layers">Layers to add.</param>
        public void AddLayers(params Layer[] layers)
        {
            foreach (Layer layer in layers)
            {
                this.layers.Add(layer);
                if (layer is GameObjectLayer)
                {
                    GameObjectLayers.Add((GameObjectLayer)layer);
                }
                else if (layer is GUILayer)
                {
                    GUILayers.Add((GUILayer)layer);
                }
            }
        }
        /// <summary>
        /// Removes the layer.
        /// </summary>
        /// <param name="name">Name of layer to remove from world.</param>
        public void RemoveLayer(string name)
        {
            foreach (Layer layer in layers)
            {
                if (layer.GetName() == name)
                {
                    layers.Remove(layer);
                    if (layer is GameObjectLayer)
                    {
                        GameObjectLayers.Remove((GameObjectLayer)layer);
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Gets the layer from world.
        /// </summary>
        /// <returns>The layer.</returns>
        /// <param name="name">Name of layer to get.</param>
        public Layer GetLayer(string name)
        {
            foreach (Layer layer in layers)
            {
                if (layer.GetName() == name)
                    return layer;
            }
            return null;
        }
        /// <summary>
        /// Returns reference of <see cref="Player"/> object from layers. If there's no player, return null.
        /// </summary>
        /// <returns>The player.</returns>
        public Player GetPlayer()
        {
            foreach (Layer layer in layers)
            {
                if (layer is GameObjectLayer)
                {
                    foreach (GameObject element in ((GameObjectLayer)layer).elements)
                    {
                        if (element is Player)
                            return (Player)element;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Gets the interactable objects.
        /// </summary>
        /// <returns>The interactable objects.</returns>
        public List<InteractableObject> GetInteractableObjects()
        {
            List<InteractableObject> result = new List<InteractableObject>();
            foreach (GameObjectLayer layer in GameObjectLayers)
            {
                foreach (GameObject element in layer.elements)
                {
                    if (element is InteractableObject)
                        result.Add((InteractableObject)element);
                }
            }

            return result;
        }
        /// <summary>
        /// Gets the enemy souls.
        /// </summary>
        /// <returns>The enemy souls.</returns>
        public List<EnemySoul> GetEnemySouls()
        {
            List<EnemySoul> result = new List<EnemySoul>();
            foreach (GameObjectLayer layer in GameObjectLayers)
            {
                foreach (GameObject element in layer.elements)
                {
                    if (element is EnemySoul)
                        result.Add((EnemySoul)element);
                }
            }

            return result;
        }
        /// <summary>
        /// Draw all layers in order.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Layer layer in layers)
            {
                layer.DrawBegin(gameTime, spriteBatch);
            }
            foreach (Layer layer in layers)
            {
                layer.Draw(gameTime, spriteBatch);
            }
        }
        /// <summary>
        /// Update all updateable layers.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            foreach (Layer layer in layers)
            {
                layer.Update(gameTime);
            }
        }
    }
}
