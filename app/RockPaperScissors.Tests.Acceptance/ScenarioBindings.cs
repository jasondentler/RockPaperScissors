using BoDi;
using Coypu;
using TechTalk.SpecFlow;

namespace RockPaperScissors.Tests.Acceptance
{
    [Binding]
    public class ScenarioBindings
    {
        private readonly IObjectContainer _objectContainer;

        public ScenarioBindings(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        private BrowserSession CreateBrowser()
        {
            var browser = new BrowserSession();
            return browser;
        }

        private void RegisterBrowser()
        {
            var browser = CreateBrowser();
            _objectContainer.RegisterInstanceAs(browser);
        }

        private void DisposeBrowser()
        {
            var browser = _objectContainer.Resolve<BrowserSession>();
            browser.Dispose();
        }

        [Before()]
        public void Before()
        {
            RegisterBrowser();
        }

        [After()]
        public void After()
        {
            DisposeBrowser();
        }

    }

}
