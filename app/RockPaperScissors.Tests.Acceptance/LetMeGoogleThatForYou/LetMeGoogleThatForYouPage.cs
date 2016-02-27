using Coypu;

namespace RockPaperScissors.Tests.Acceptance.LetMeGoogleThatForYou
{
    public class LetMeGoogleThatForYouPage : PageObject
    {
        public LetMeGoogleThatForYouPage(BrowserSession browser) : base(browser)
        {
        }

        private static class Locators
        {
            public const string SearchTextbox = "search-term";
            public const string SearchButton = "search";
            public const string LinkTextbox = "link input[type='text']";
            public const string CopyButton = "copy";
            public const string ShortenButton = "shorten";
            public const string PreviewButton = "preview";
        }

        public void GoToPage()
        {
            Browser.Visit("http://lmgtfy.com");
        }

        public void Search(string search)
        {
            Browser.FillIn(Locators.SearchTextbox).With(search);
            Browser.ClickButton(Locators.SearchButton);
        }

        public ElementScope LinkTextbox
        {
            get { return Browser.FindCss(Locators.LinkTextbox); }
        }

        public ElementScope CopyButton
        {
            get { return Browser.FindCss(Locators.CopyButton); }
        }

        public ElementScope ShortenButton
        {
            get { return Browser.FindCss(Locators.ShortenButton); }
        }

        public ElementScope PreviewButton
        {
            get { return Browser.FindCss(Locators.PreviewButton); }
        }

    }
}
