Description : 

Prototype RTS 3D médieval réalisé avec Unity(non aboutie). Les objectifs et attendus du jeu ne sont pas encore clairement définit, ayant été initié il y a peu de temps, le but étant de construire un prototype qui servira de base pour un projet plus ambitieux.

Grand fan de jeu de gestion, de l'époque médiéval et passionné de développement, j'ai de nombreuses idées en terme de gameplay et de design mais pour le moment, il s'agit de concevoir les première mécaniques de gameplay, 
tout en commençant à réfléchir et travailler sur quelques designs afin de développer mes compétences sur Blender tout en essayant des styles, ce qui affinera progressivement les objectifs de ce projet.

Il s'agit de mon projet principal.

Je tiens à préciser que je m'inspire de tutoriels et guides proposés par d'autres, dans le cadre de mon activité, c'est dans une démarche d'apprentissage, à savoir que majoritairement, je modifie 
ce que je trouve car j'aime apporter ma touche. Et, je n'utilise que mes assets(dessin, modèle 3D etc...)

Actuellement, seul l'état actuel du projet est présenté, mais, ayant fait du versionning, je ne manquerais pas d'enrichir l'historique afin de garantir un meilleur suivi, dès que possible.

Vous pouvez accéder aux modèles 3D utilisés pour ce projet à cette adresse : https://github.com/sbugeas/3D_Modeles_GIT



ACHEVÉ :

- Ajout gestion de la Caméra
- Ajout sélection simple et multiple d'unité (Click + Drag Selection, avec limite imposée)
- Déplacement seul/en groupe d'unité(s) (via Navmesh)
- Correction Drag Selection -> Le rend dynamique (unités sélectionnées constamment MAJ pendant le drag)
- Bases du mode construction. Proposition placement avec contrôle du terrain puis placement(à finir)
- Réorganisation pour concevoir système de combat (retrait marqueur position et formation des unités auparavant implémenté car trop complexe et mal optimisé)
- Début du système de combat (Détection ennemi, following(automatiquement et sur commande du joueur))
- Ajout système d'état (Attack, Following, Idle) + retrait des points de vie de ennemi lors d'une attaque + slider(UI)
- Modèle Soldier(première unité de combat) créée sur Blender(pourra évoluer)
- Préparation animations d'attente, de course, de garde, d'attaque, et de mort(via Blender)
- Ajout du curseur d'origine ainsi que le curseur de combat

EN COURS :
- Positionnement(formation) des unités suite à un déplacement en groupe

PRÉVISIONS :
- Modèlisation + Rigging + Animation + Import Unity(et config) d'une nouvelle unité, l'archer
- 

Outils :

- Unity
- Blender
- Visual Studio(C#)


Sources :

[Make RTS (Mike)](https://www.youtube.com/watch?v=-GfdKB_7mrY&list=PLtLToKUhgzwkCRQ9YAOtUIDbDQN5XXVAs)

[Navmesh (Tuto Unity FR)](https://www.youtube.com/watch?v=qOQVxPQ-C5Y&t=489s)

[Unity - changer le curseur de la souris(MakeYourGame)](https://www.youtube.com/watch?v=qifz_CXe4CQ&t=321s)

