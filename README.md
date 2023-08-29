# ScoreBoard assumptions:
1. No team can play more than 1 game at the same time;
2. UpdateScore is executed post every goal even if there're two goals scored very quickly one after another;
3. Scores are updated once a goal is officially/finally confirmed (and post VAR check);
4. UpdateScore in addition to a pair of absolute scores (as per requirements) needs to have some match identifier thus matchId was added as parameter;

#Potential improvements:
1. Customize exception handling e.g. separate exception type
