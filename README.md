# poker-card-winner
Exercise to get the winner of the poker play.

We've created a simple multiplayer card game where:
7 players are dealt 5 cards from two 52 card decks, and the winner is the one with the highest score.
The score for each player is calculated by adding the highest 3 card values for each player, where the
number cards have their face value, J = 11, Q = 12, K = 13 and A = 11 (not 1).
In the event of a tie, the scores are recalculated for only the tied players by calculating a "suit score" for
each player to see if the tie can be broken (it may not).
Each card is given a score based on its suit, with diamonds = 1, hearts = 2, spades = 3 and clubs
= 4, and the player's score is the suit value of the player’s highest card.

Command Parameters
• The command parameters can be in any order.
• The file names can vary.
• It can be assumed that if they are not located in the same directory as the application, that the path will
be supplied as part of the file name.

--in abc.txt --out xyz.txt

Input File Structure
• The input file will contain 7 rows, one for each player's hand of 5 cards.
• Each row will contain the player's name separated by a colon then a comma separated list of the 5 cards.
• Each card will consist of the face value and the suit (H = Hearts, S = Spades, C = Clubs and D = Diamonds).

• Example: KD = King of Diamonds.
• Input is not case-sensitive.
• Spaces can be ignored.

Example input:
Player1:2S,6S,3C,JS,3S
Player2:5C,5H,9H,7D,8D
Player3:5D,8D,4D,10H,7D
Player4:5S,9H,8H,10C,8S
Player5:8C,2H,7H,QC,6D
Player6:4H,JC,9C,JH,10D
Player7:JS,KS,4H,10C,3C
