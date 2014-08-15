using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Reportazhyst.WP8
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void EmailMe_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                EmailComposeTask email = new EmailComposeTask();
                email.To = "vbilenko@outlook.com";
                email.Subject = "[Marketplace][WP7] Reportazhyst";
                email.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Email error", MessageBoxButton.OK);
            }
        }

        private void WriteReview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                MarketplaceReviewTask review = new MarketplaceReviewTask();
                review.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Marketplace error", MessageBoxButton.OK);
            }
        }

    }
}