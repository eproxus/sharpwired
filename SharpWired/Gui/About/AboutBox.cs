using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SharpWired.Gui.Forms;

namespace SharpWired.Gui.About {
    internal partial class AboutBox : WiredForm {
        public AboutBox() {
            InitializeComponent();

            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;

            ReadLicense();
        }

        private void ReadLicense() {
            var licenseFile = Path.Combine(Application.StartupPath, "LICENSE.txt");
            if (File.Exists(licenseFile)) {
                var license = File.ReadAllText(licenseFile);
                textBoxDescription.Text = license;
            }
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle {
            get {
                // Request all Title attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0) {
                    // Select the first one
                    var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    }
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string AssemblyDescription {
            get {
                // Request all Description attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                // If there aren'transfer any Description attributes, return an empty string
                if (attributes.Length == 0) {
                    return "";
                }
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        public string AssemblyProduct {
            get {
                // Request all Product attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                // If there aren'transfer any Product attributes, return an empty string
                if (attributes.Length == 0) {
                    return "";
                }
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        public string AssemblyCopyright {
            get {
                // Request all Copyright attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                // If there aren'transfer any Copyright attributes, return an empty string
                if (attributes.Length == 0) {
                    return "";
                }
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany {
            get {
                // Request all Company attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
                // If there aren'transfer any Company attributes, return an empty string
                if (attributes.Length == 0) {
                    return "";
                }
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        #endregion
    }
}