using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaChallengeWar
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PlayWarButton_Click(object sender, EventArgs e)
        {
            List<Player> players = new List<Player>()
            {
                new Player("Ken"),
                new Player("Bob"),

                // TODO: Fix the Game class, so that playing with more than 2 players 
                // returns the correct scores when a player runs out of cards
                //new Player("Bill"),
                //new Player("Steve")
            };

            Game myGame = new Game(players);

            resultLabel.Text = myGame.PlayWar();
        }
    }
}