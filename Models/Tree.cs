using System;
using System.Collections.Generic;
using System.Linq;
using test.back.devises.Resources;

namespace test.back.devises.Models
{
    internal class Tree
    {
        private TreeNode _rootNode { get; set; }

        public Tree(string rootNode)
        {
            _rootNode = new TreeNode()
            {
                Depth = 0,
                Value = rootNode
            };
        }

        #region Private Methods

        private IEnumerable<TreeNode> GetChildren(IEnumerable<Currency> currencies, TreeNode parent)
        {
            IEnumerable<TreeNode> children = new List<TreeNode>();
            IEnumerable<Currency> childrenCurrencies;
            TreeNode currentParent = parent;
            int depth = parent.Depth + 1;

            // Recherche dans la colonne de devise de départ / arrivée
            if (depth % 2 == 1)
            {
                childrenCurrencies = currencies.Where(x => string.Equals(x.IncomingName, currentParent.Value) && !string.Equals(x.ArrivalName, currentParent.Parent?.Value));
            }
            else
            {
                childrenCurrencies = currencies.Where(x => string.Equals(x.ArrivalName, currentParent.Value) && !string.Equals(x.IncomingName, currentParent.Parent?.Value));
            }

            foreach (var currency in childrenCurrencies)
            {
                var node = new TreeNode()
                {
                    Depth = depth,
                    Value = depth % 2 == 1 ? currency.ArrivalName : currency.IncomingName,
                    Parent = currentParent,
                };

                node.Children = GetChildren(currencies, node);

                children = children.Append(node);
            }

            return children;
        }

        private LinkedList<TreeNode> GetShortestPath(IEnumerable<TreeNode> nodes, string value)
        {
            var shortestPath = new LinkedList<TreeNode>();

            foreach (var node in nodes)
            {
                // On recupere le chemin parcouru
                var currentPath = GetCurrentPath(node);

                // On trouve un chemin potentiel
                if (string.Equals(node.Value, value))
                {
                    if (!shortestPath.Any())
                    {
                        shortestPath = currentPath; // Aucun chemin trouve precedemment on assigne le chemin trouve
                        continue;
                    }
                    else
                    {
                        if (currentPath.Last().Depth < shortestPath.Last().Depth) // Le chemin est plus court que celui deja trouve donc on change le chemin le plus court
                            shortestPath = currentPath;
                    }
                    continue;
                }

                // On continue à chercher dans les nodes enfants
                if (node.Children != null && !node.Children.Any())
                    continue;

                var childrenShortestPath = GetShortestPath(node.Children!, value);
                if (!shortestPath.Any() && childrenShortestPath.Any())
                {
                    shortestPath = childrenShortestPath; // Aucun chemin trouve precedemment on assigne le chemin trouve
                    continue;
                }
                else
                {
                    if (childrenShortestPath.Any() && childrenShortestPath.Last().Depth < shortestPath.Last().Depth) // Le chemin est plus court que celui deja trouve donc on change le chemin le plus court
                        shortestPath = childrenShortestPath;
                }
            }

            return shortestPath;
        }

        private static LinkedList<TreeNode> GetCurrentPath(TreeNode node)
        {
            var currentNode = node;

            var path = new LinkedList<TreeNode>();

            while (currentNode != null)
            {
                path.AddFirst(currentNode);
                currentNode = currentNode.Parent;
            }

            return path;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Construit tous les chemins possibles à partir de la devise de départ (de la devise cible) en fonction du tableau de devises reçu
        /// </summary>
        /// <param name="currencies"></param>
        public void Build(IEnumerable<Currency> currencies)
        {
            _rootNode.Children = GetChildren(currencies, _rootNode);
        }

        /// <summary>
        /// Parcourt l'ensemble des chemins de l'arbre en pronfondeur (DFS) : https://www.jesuisundev.com/comprendre-les-algorithmes-de-parcours-en-8-minutes/
        /// Et retourne les noeuds du chemin le plus court
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public LinkedList<TreeNode> GetShortestPath(string value)
        {
            if (_rootNode.Children == null)
                throw new Exception(Errors.Tree_NotBuild);

            return GetShortestPath(_rootNode.Children, value);
        }

        #endregion
    }
}
