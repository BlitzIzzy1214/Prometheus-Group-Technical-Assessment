using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

/**
 * Test suite using Playwright with C# to traverse from Google to Prometheus Group's website, and then attempt to submit an incorrect form
 */
namespace PlaywrightTest
{

    /**
     * Class storing the test to traverse the Prometheus Group website
     */
    public class PlaywrightTests : PageTest
    {

        /**
         * Tests traversal from Google to the Prometheus Group Contact Us page, and submission of an incorrect form.
         * Includes testing for visibly required fields, Alerts when submitting incorrectly, and negative testing for fields that are not required.
         */
        [Fact]
        public async Task TestContactUs()
        {
            // Open Google and search "Prometheus Group"
            await Page.GotoAsync("https://www.google.com/");
            await Page.GetByRole(AriaRole.Combobox, new() { Name = "Search" }).ClickAsync();
            await Page.GetByRole(AriaRole.Combobox, new() { Name = "Search" }).FillAsync("Prometheus Group");

            /**
             * Initial attempt to do a regular Google search and click on the Contact Us link.
             * This is the point where it gets stuck on the Google CAPTCHA.
             */
            // await Page.GotoAsync("https://www.google.com/search?q=Prometheus+Group&sca_esv=39b731e95fabc852&source=hp&ei=_2D5aO3HFMSjqtsP5OWi8Qo&iflsig=AOw8s4IAAAAAaPlvD6sg6fBFSfLW6RSYVyC4Eubdjnmb&ved=0ahUKEwitw8DC87iQAxXEkWoFHeSyKK4Q4dUDCBA&uact=5&oq=Prometheus+Group&gs_lp=Egdnd3Mtd2l6IhBQcm9tZXRoZXVzIEdyb3VwMg0QABiABBixAxhDGIoFMgsQLhiABBjHARivATIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgAQyBRAAGIAESLwtUJcEWMkjcAV4AJABAJgBf6AB7g6qAQQxNy4zuAEDyAEA-AEBmAIZoAKuD6gCCsICFhAuGIAEGEMYtAIYxwEYigUY6gIYrwHCAhAQABiABBhDGLQCGIoFGOoCwgIWEC4YgAQY0QMYQxi0AhjHARiKBRjqAsICGhAAGIAEGLQCGNQDGOUCGLcDGIoFGOoCGIoDwgIKEAAYgAQYQxiKBcICEBAuGIAEGEMYxwEYigUYrwHCAgoQLhiABBhDGIoFwgINEC4YgAQYsQMYQxiKBcICCBAAGIAEGLEDwgIKEAAYgAQYsQMYCsICChAuGIAEGLEDGArCAg0QABiABBixAxiDARgKwgIQEAAYgAQYsQMYgwEYigUYCsICBxAAGIAEGArCAggQLhiABBixA8ICCxAAGIAEGLEDGIMBwgIOEAAYgAQYsQMYgwEYigWYAwPxBVoRqStqg9qHkgcEMjEuNKAHrsMBsgcEMTYuNLgHpQ_CBwYxLjIyLjLIBys&sclient=gws-wiz");
            // await Expect(Page.GetByRole(AriaRole.Combobox, new() { Name = "Search" })).ToHaveValueAsync("Prometheus Group");
            // await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Prometheus Group: Asset" })).ToBeVisibleAsync();
            // await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Contact Us" })).ToBeVisibleAsync();
            // await Page.GetByRole(AriaRole.Link, new() { Name = "Contact Us" }).ClickAsync();

            // Go to Prometheus Group's Contact Us page
            await Page.GotoAsync("https://www.prometheusgroup.com/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Contact Sales" }).ClickAsync();

            // Check for visible components on the page, specifically the * marking required fields
            await Expect(Page.Locator("label").Filter(new() { HasText = "First Name*" }).Locator("span").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("label").Filter(new() { HasText = "Last Name*" }).Locator("span").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("label").Filter(new() { HasText = "Business Email Address*" }).Locator("span").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("label").Filter(new() { HasText = "Phone number*" }).Locator("span").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("label").Filter(new() { HasText = "Company name*" }).Locator("span").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("#label-global_region-fe70f03d-5bac-4ad3-a698-3e130182d674_7317").GetByText("*")).ToBeVisibleAsync();
            await Expect(Page.Locator("#label-my_primary_system-fe70f03d-5bac-4ad3-a698-3e130182d674_7317").GetByText("*")).ToBeVisibleAsync();
            await Expect(Page.Locator("label").Filter(new() { HasText = "What product are you interested in?*" }).Locator("span").Nth(1)).ToBeVisibleAsync();

            // Check that the required fields have the required attribute
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name*" })).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name*" })).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Business Email Address*" })).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Phone number*" })).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Company name*" })).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByLabel("Global Region*")).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByLabel("My Primary System*")).ToHaveAttributeAsync("required", "");
            await Expect(Page.GetByLabel("What product are you interested in?*")).ToHaveAttributeAsync("required", "");

            // Check that the final field, the "Additional Comments or Questions" box, has no indication of being required (i.e., no *, and no required attribute)
            await Expect(Page.Locator("#label-additional_comments_or_questions-fe70f03d-5bac-4ad3-a698-3e130182d674_7317").GetByText("*")).ToHaveCountAsync(0);
            await Expect(Page.GetByLabel("Additional Comments or Questions:")).Not.ToHaveAttributeAsync("required", "");

            // Fill out the First Name and Last Name fields
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name*" }).FillAsync("First");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name*" }).FillAsync("Last");

            // Leave the rest of the fields blank
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Business Email Address*" })).ToBeEmptyAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Phone number*" })).ToBeEmptyAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Company name*" })).ToBeEmptyAsync();
            await Expect(Page.GetByLabel("My Primary System*")).ToHaveValueAsync("");
            await Expect(Page.GetByLabel("Global Region*")).ToHaveValueAsync("");
            await Expect(Page.GetByLabel("What product are you interested in?*")).ToHaveValueAsync("");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Additional Comments or Questions:" })).ToBeEmptyAsync();

            // Attempt to submit
            await Page.GetByRole(AriaRole.Button, new() { Name = "Contact Us" }).ClickAsync();

            // No redirection, check that the warnings appeared after attempting to submit incorrect information (search by text)
            await Expect(Page.GetByText("Please complete all required")).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"]")).ToContainTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Business Email Address*Please enter a valid business email.Please complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Phone number*Please complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Company name*Please complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Global Region*Please SelectEMEALATAMNorth AmericaAPACPlease complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "My Primary System*Please SelectSAPIBM MaximoOracleOtherPlease complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "What product are you interested in?*Please SelectPlanning & SchedulingMobilityPermitting and SafetyReporting and AnalyticsShutdown, Turnaround & OutagePrometheus RiskPoyntConstruction and Commissioning ManagementContractor ManagementAPMCapital ProjectsMaster Data as a ServiceMaster Data GovernanceAsset Information Workbench Retail Fashion ManagementPlease complete this required field." }).Locator("label").Nth(1)).ToBeVisibleAsync();

            // Check specifically for Alert items in the invalid fields
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Business Email Address*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Phone number*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Company name*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Global Region*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "My Primary System*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "What product are you interested in?*" }).GetByRole(AriaRole.Alert)).ToHaveTextAsync("Please complete this required field.");

            // Check that the First Name, Last Name, and Additional Comments or Questions sections do not have an Alert since they were filled out/not required
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "First Name*" }).GetByRole(AriaRole.Alert)).ToHaveCountAsync(0);
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Last Name*" }).GetByRole(AriaRole.Alert)).ToHaveCountAsync(0);
            await Expect(Page.Locator("[data-test-id=\"hsForm_fe70f03d-5bac-4ad3-a698-3e130182d674_7317\"] div").Filter(new() { HasText = "Additional Comments or Questions:" }).GetByRole(AriaRole.Alert)).ToHaveCountAsync(0);

            // Check to see if the fields were left the same as they were before submission (i.e., name fields are filled, the rest are empty)
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name*" })).ToHaveValueAsync("First");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name*" })).ToHaveValueAsync("Last");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Business Email Address*" })).ToBeEmptyAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Phone number*" })).ToBeEmptyAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Company name*" })).ToBeEmptyAsync();
            await Expect(Page.GetByLabel("My Primary System*")).ToHaveValueAsync("");
            await Expect(Page.GetByLabel("Global Region*")).ToHaveValueAsync("");
            await Expect(Page.GetByLabel("What product are you interested in?*")).ToHaveValueAsync("");
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Additional Comments or Questions:" })).ToBeEmptyAsync();
        }
    }
}
