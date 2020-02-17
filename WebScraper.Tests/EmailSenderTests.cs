using Microsoft.VisualStudio.TestTools.UnitTesting;
using NS_EmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_EmailSender.Tests
{
    [TestClass()]
    public class EmailSenderTests
    {
        [TestMethod()]
        public void AddTextTest()
        {

            //Arrange
            var emailer = new EmailSender();
            //Act
            emailer.AddText("The dog ate the cat.");
            emailer.AddText("\n The cat ate the mouse.");
            //Assert
            emailer.SendMail("Test_Newline", emailer.SavedMessageBody);

        }
    }
}