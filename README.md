# ScoreBoard assumptions:
1. Two same teams can't play more than 1 game at the same time;
2. UpdateScore is executed post every goal even if there're two goals scored very quickly one after another;
3. Scores are updated once a goal is officially/finally confirmed (and post VAR check);

#Potential improvements:
1. Customize exception handling e.g. separate exception type
2. Separate validation logic from controller 
