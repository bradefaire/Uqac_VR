# Quickdraw VR

**Projet universitaire** — Jeu VR sous Unity visant à mesurer le temps de réaction et la précision pour différentes positions initiales de la manette. L'objectif est d'identifier la position la plus performante à l'aide d'analyses statistiques sur les données collectées.

## 📜 Description Technique

### Architecture générale

- **Plateforme** : Unity 6
- **Langages utilisés** : C# (Unity Scripts) + Python (Analyse des données)
- **Export des données** : Automatique à la fermeture de l'application (format `.csv`)

### Organisation du projet

```
Assets/
├── Scripts/                → Scripts C# du jeu (tir, chronométrage, export CSV)
├── Scenes/                 → Scènes Unity (.unity) du jeu
├── Assets/                 → Ressources 3D (modèles de revolver et cible)
├── Sounds/                 → Sons émis en jeu (signal de tir, bruits du pistolet)
Resultats/
├── DATE_TIME.csv           → Données de tirs exportées
├── traitement_donnees.py   → Script Python pour l'analyse statistique
├── Figure_X.png            → Graphiques générés (moyennes/écarts-types)
```

### Fonctionnement du projet

1. **Jeu VR** :  
   - L'utilisateur équipe différentes positions de départ via des **holsters imprimés en 3D** adaptés aux manettes Meta Quest 3.
   - À chaque session, il peut appuyer sur *Start* pour activer un **timer** et recevoir un **signal sonore** indiquant de tirer.
   - **Chaque tir** est enregistré avec :
     - le temps de réaction,
     - la précision du tir (ou `-1` si le tir n'était pas chronométré).

2. **Collecte de données** :  
   - À la fermeture du jeu, les données sont automatiquement sauvegardées dans un fichier `.csv`.

3. **Analyse des données** :
   - Ajout manuel d'une colonne indiquant la position utilisée pour chaque tir.
   - **Script Python** (`traitement_donnees.py`) :
     - Exclusion des tirs non chronométrés (précision de `-1`).
     - Comparaison statistique des positions : test de **Wilcoxon** (2 positions) ou **Friedman** (>2 positions).
     - Génération de graphiques représentant : moyennes et écarts-types des temps de réaction selon la distance de tir


## 🔧 Dépendances

- **Unity** 2022+ avec support VR
- **Python 3.8+** avec :
  - `pandas`
  - `scipy`
  - `matplotlib`
  - `seaborn`

Installation rapide des dépendances Python :
```
pip install pandas scipy matplotlib seaborn
```


## 📈 Exemple de flux d'utilisation

1. Lancer l'application VR et effectuer des séries de tirs pour chaque position.
2. Fermer l'application → Génération automatique du fichier `DATE_TIME.csv`.
3. Ajouter manuellement la colonne "position" (ex : Position ceinture, flanc, dos).
4. Exécuter le script Python pour :
   - Analyser statistiquement les performances par position.
   - Générer les graphiques associés.


## 💬 Remarques

- Le processus de tir sans appui préalable sur Start génère des données ignorées dans l'analyse.
- L'analyse ne prend en compte que le temps de réaction, il faut donc privilégier la rapidité d'exécution à la visée lors des tests utilisateur.
- Une quantité **importante de tirs** est recommandée pour assurer la fiabilité statistique des résultats, le cas échéant les graphes permettent tout de même une analyse (les temps moyens sont des bons indicateurs de performance).
