# TP Modélisation Géométrique : Modèles Volumiques

**Adjamé Tellier-Rozen**  
**ID étudiant :** 2235301  

## Description du Projet
Ce projet est le rendu du TP3 pour la matière modélisation géométrique

## Structure du Projet

### Exo1
Les fichiers de `Exo1` correspondent aux 5 premières questions de l'exo 1 il n'y a pas la question bonus je n'ai pas réussi a l'implémenter

- **Fichier de Scène :** `Exo1.unity`
- **Scripts :**
  - **Gestion.cs** : Initialise les sphères et les octrees pour le modèle de base.
  - **OcTree.cs** : Contient les fonctions de base pour la création et manipulation de l’octree.
  - **SphereManager.cs** : Coordonne le tout

### Exo2
Les fichiers de `Exo2` correspondent à l'exo 2

- **Fichier de Scène :** `Exo2.unity`
- **Scripts :**
  - **OctreeExo2.cs** : Gère la structure avancée de l’octree pour la spatialisation des voxels.
  - **SphereControllerExo2.cs** : Contrôle une sphère avec les touches directionnelles, ajoutant de la matière aux voxels situés en dessous.
  - **VoxelManagerExo2.cs** : Gère les voxels générés et leur affichage selon les interactions de l’utilisateur.

## Démarrage

Pour lancer le projet :

1. **Ouvrez Unity**.
2. Dans Unity, ouvrez le dossier **TP3** pour accéder à tous les fichiers nécessaires.
3. Sélectionnez la scène correspondant à l'exercice que vous souhaitez exécuter (`Exo1.unity` ou `Exo2.unity`).

## Utilisation
- Dans `Exo1`, lancez et dans la scène tournez autour des différentes sphères vous pouvez choisir le nombre qui apparait et leur taille pour la resolution c'est mieux de la cahnger dans le code precisement
- Dans `Exo2`, utilisez les touches directionnelles pour contrôler la sphère et interagir avec les voxels. zqsd et a/e pour l'axe y, de plus appuyez sur espace pour passer de ajouter de la matièere à supprimer de la matière
