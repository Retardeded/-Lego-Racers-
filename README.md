# -Lego-Racers-

Link to exemplary gameplay https://www.youtube.com/watch?v=1AS8kLvLBPc&t=5s
 
I am only a beginner in game development, this is only my second game, so while creating it I was mainly focus on making it's core mechanics work properly so that my game with right level design is potentialy playable. So this game does not look good at all, but so far im ok with that.
#include <stdio.h>

int main(void)
 {
   int tab[10];
   int n = 9;
   for(int i = 0; i < 9; i++)
   {
       scanf ("%d", &tab[i]);
       if(i == 0)
       {
           printf("%i", tab[i]);
       }
       else if(i == 1)
       {
           printf("%o", tab[i]);
       }
       else if(i == 2)
       {
           printf("%x", tab[i]);
       }
       else if(i == 3)
       {
           printf("%u", tab[i]);
       }
       else if(i == 4)
       {
           if(tab[i] >= 0)
           {
               printf("%x", tab[i]);
           }
           else
           {
               printf("-");
               printf("%x", tab[i]);
           }
       }
       else if(i == 5)
       {
           printf("%f", tab[i]);
       }
       else if(i == 6)
       {
           printf("%+.f", tab[i]);
       }
       else if(i == 7)
       {
           printf("%+.f", tab[i]);
       }
       else if(i == 8)
       {
           printf("%+.f", tab[i]);
       }
       printf(" ");
   }
   return 0;
 }
//author: Piotr Kurek

#include <stdio.h>
#include <math.h>

int main(void)
{

int toSeconds= 60*60;
int toSecondsPerDay = toSeconds * 24;
int toSecondsPerWeek = toSecondsPerDay * 7;
int Hours, n;

Hours = scanf("%i", &n);
printf("space  ");
printf("%i", Hours);
printf(" space ");
int seconds = Hours*toSeconds;
printf("%i hours = %i", Hours, seconds);

}
