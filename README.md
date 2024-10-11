
# QuizApp - Application de Quiz Multijoueur de Bureau en Java et .NET

QuizApp est une application de quiz multijoueur de bureau, conçue en utilisant Java pour le backend (communication et logique serveur) et .NET C# pour l’interface utilisateur sur le bureau. Ce projet permet à deux joueurs de s'affronter en répondant à des questions de culture générale tout en communiquant via un chat intégré. L’application allie la puissance de Java pour le traitement côté serveur avec la flexibilité de .NET pour une interface utilisateur réactive et intuitive.

## Fonctionnalités

-   **Quiz de culture générale** : Quiz de questions à choix multiples couvrant divers domaines, comme l’histoire, la science, la géographie, etc.
-   **Interface de bureau en .NET C#** : Une interface graphique interactive et conviviale, développée en .NET C#, pour une expérience utilisateur fluide et moderne sur le bureau.
-   **Système de points** : Les bonnes réponses rapportent des points (10 points par réponse correcte). Si un joueur se trompe, l'autre joueur gagne automatiquement les points.
-   **Chat intégré en temps réel** : Les joueurs peuvent échanger des messages pendant le quiz via une fonctionnalité de chat intégrée dans l'interface.
-   **Gestion des connexions** : Le jeu commence automatiquement une fois que deux joueurs sont connectés. Les connexions supplémentaires recevront un message de limitation du nombre de joueurs.
-   **Annonce des scores et du gagnant** : À la fin du quiz, l'application affiche le gagnant avec le score final de chaque joueur.

## Architecture du Projet

-   **Langages de programmation** : Java pour le serveur et .NET C# pour l’interface de bureau.
-   **Interface graphique de bureau** : Développée en .NET C# pour une interface utilisateur moderne et efficace sur Windows.
-   **Communication par sockets** : Utilisation de sockets TCP pour permettre une communication en temps réel entre le serveur Java et les clients de bureau .NET.
-   **Multi-threading** : Chaque client est géré dans un thread distinct pour permettre une communication simultanée et fluide avec le serveur.
-   **Gestionnaire de questions** : La banque de questions est stockée côté serveur, avec chaque question comportant des options de réponse et une réponse correcte.

## Classes principales

1.  **QuizServer** (Java) :
    
    -   Démarre le serveur et écoute les connexions sur un port spécifique.
    -   Accepte jusqu'à deux joueurs et lance automatiquement le quiz dès que deux joueurs sont connectés.
    -   Gère l'envoi des questions, vérifie les réponses et met à jour les scores en temps réel.
    -   Gère la diffusion des messages de chat entre les joueurs connectés.
2.  **QuizClient** (Java et C# .NET) :
    
    -   La partie Java assure la logique de connexion et la gestion des réponses.
    -   L'interface de bureau en C# permet aux utilisateurs de sélectionner des réponses, afficher les scores et envoyer des messages dans le chat intégré.
    -   Les scores et messages sont mis à jour en temps réel via la communication par sockets.
3.  **Interface utilisateur en C#** :
    
    -   Affiche les questions et les options de réponse de manière interactive.
    -   Comprend des boutons pour sélectionner les réponses et une section de chat pour les échanges en temps réel.
    -   Indique les scores des joueurs et affiche le gagnant à la fin de la partie.

## Exécution

1.  **Démarrage du serveur** : Exécutez `QuizServer` en Java pour démarrer le serveur sur un port prédéfini.
2.  **Lancement de l'interface client** : Chaque joueur lance l'interface de bureau en C# pour se connecter au serveur.
3.  **Interaction avec l'interface** : Les joueurs répondent aux questions en utilisant les boutons d'option, et les scores sont mis à jour instantanément.
4.  **Chat en jeu** : Les joueurs peuvent envoyer des messages via le chat en temps réel pendant la partie.
5.  **Fin du quiz** : Le gagnant et les scores sont affichés à la fin du quiz dans l'interface .NET.

## Objectif

L'objectif de QuizApp est d'offrir une application de quiz multijoueur de bureau qui combine un backend puissant en Java et une interface utilisateur intuitive en .NET C#. Cette application permet de tester les connaissances de manière ludique et interactive, tout en facilitant la communication entre les joueurs grâce au chat intégré.