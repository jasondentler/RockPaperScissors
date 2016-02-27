using Coypu;
using TechTalk.SpecFlow;
using Coypu.NUnit;
using SharpTestsEx;

namespace RockPaperScissors.Tests.Acceptance.LetMeGoogleThatForYou
{
    [Binding]
    public class LetMeGoogleThatForYouSteps
    {
        private readonly LetMeGoogleThatForYouPage _page;

        public LetMeGoogleThatForYouSteps(BrowserSession browser)
        {
            _page = new LetMeGoogleThatForYouPage(browser);
        }

        [Given(@"I am on Let Me Google That For You")]
        public void GivenIAmOnLetMeGoogleThatForYou()
        {
            _page.GoToPage();
        }
        
        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchText)
        {
            _page.Search(searchText);
        }
        
        [Then(@"the link to that search is displayed")]
        public void ThenTheLinkToThatSearchIsDisplayed()
        {
            _page.LinkTextbox.Exists();
        }
        
        [Then(@"the copy button is visible")]
        public void ThenTheCopyButtonIsVisible()
        {
            _page.CopyButton.Exists();
        }
        
        [Then(@"the shorten button is visible")]
        public void ThenTheShortenButtonIsVisible()
        {
            _page.ShortenButton.Exists();
        }
        
        [Then(@"the preview button is visible")]
        public void ThenThePreviewButtonIsVisible()
        {
            _page.PreviewButton.Exists();
        }
    }
}
