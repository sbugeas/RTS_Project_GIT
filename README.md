--- DESCRIPTION ---

Projet 3D personnel qui est un prototype RTS (Real Time Strategy) solo s’inscrivant dans un univers médiéval. 
Le joueur devra développer sa colonie, faire prospérer son peuple et le défendre face aux attaques de pillards.


--- CONTRÔLES ---


Flèches directionnelles (↓↑←→) : Déplacer la caméra

Molette souris : Zoom caméra

Molette souris(rester appuyé) : Rotation caméra

T : Construire une maison (Temporaire/Pour tests)

Clique gauche : Sélectionne unité(militaire)

Clique droit(unité(s) sélectionnée(s)) : Déplacement seul ou en groupe(formation)

Clique droit(unité(s) militaire(s) sélectionnée(s)) sur cible ennemie : Commande d'attaque



--- ACHEVÉS ---

- Gestion de la Caméra (déplacemebt, rotation, zoom)
- Système de sélection unique et multiple d'unité(s)
- Système de déplacement d’une ou plusieurs unités (via Navmesh)
- Soldat : Modélisation, import et configuration, animations(Attente, course, garde et attaque), UI(barre de vie)
- Système de combat rapproché du soldat(Détection d’ennemi et suivi, automatiquement ou sur commande du joueur)
- Ajout du curseur "de base" ainsi que le curseur de combat (momentanément retiré mais présents dans le dossier du projet)
- Système de positionnement(en formation) des unités suite à un déplacement en groupe
- Système de récolte du bois (Sera également utilisée pour la récolte de la pierre et de l'or)
- Bûcheron : Modélisation, import et configuration, animations (attente, marche, coupe, transport de rondin)
- Arbre : Modélisation, import et configuration, animation(chute)
- Modèle du hall (bâtiment principal) et de l'entrepôt
- Camp de bûcheron : Modélisation, import et configuration, UI
- Système de récolte de la pierre (légèrement différent de la récolte du bois)
- Pioche
- Animation minage
- Rocher : Modélisation, import et configuration
- Cabane de mineur de pierre (Stone miner's hut) : Modélisation, import et configuration, UI 
- Système de construction (sur terrain plat) + UI du menu de construction 
- Caserne : Modélisation, import et configuration, UI, logique de recrutement

--- PRÉVISIONS ---
- Réorganisation : Changement du système de construction pour utiliser une grille de placement --> en cours
- Cabane de mineur d'or(Gold miner's hut) : Modèlisation, import et configuration, UI
- Mineurs d'or : logique(states), animation(transport de minerai)
- Amélioration de la logique du combat pour permettre l'attaque de bâtiments (sera utilisée pour les pillards)
  


--- OUTILS ---

- Unity
- Blender
- Visual Studio(C#)



--- SOURCES ---

[Make RTS (Mike)](https://www.youtube.com/watch?v=-GfdKB_7mrY&list=PLtLToKUhgzwkCRQ9YAOtUIDbDQN5XXVAs)

[Navmesh (Tuto Unity FR)](https://www.youtube.com/watch?v=qOQVxPQ-C5Y&t=489s)

[Unity - changer le curseur de la souris(MakeYourGame)](https://www.youtube.com/watch?v=qifz_CXe4CQ&t=321s)

[Documentation Unity](https://docs.unity3d.com/Manual/index.html)

