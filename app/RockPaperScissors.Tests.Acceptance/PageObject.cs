using Coypu;

namespace RockPaperScissors.Tests.Acceptance
{
    public abstract class PageObject : IPageObject
    {
        protected PageObject(BrowserSession browser)
        {
            Browser = browser;
        }

        public BrowserSession Browser { get; }
    }
}