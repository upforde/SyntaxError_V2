﻿#pragma checksum "C:\Users\upfor\Desktop\Skole\DotNET\SyntaxError_V2\SyntaxError.V2\SyntaxError.V2.App\Views\GamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F616A5FD60699B9D934BA9B5F23FFDC7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SyntaxError.V2.App.Views
{
    partial class GamePage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class GamePage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IGamePage_Bindings
        {
            private global::SyntaxError.V2.App.Views.GamePage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ListView obj3;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj3ItemsSourceDisabled = false;

            public GamePage_obj1_Bindings()
            {
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 221 && columnNumber == 46)
                {
                    isobj3ItemsSourceDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3: // Views\GamePage.xaml line 221
                        this.obj3 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    default:
                        break;
                }
            }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                throw new global::System.NotImplementedException();
            }

            public void Recycle()
            {
                throw new global::System.NotImplementedException();
            }

            // IGamePage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::SyntaxError.V2.App.Views.GamePage)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::SyntaxError.V2.App.Views.GamePage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_ViewModel(global::SyntaxError.V2.App.ViewModels.GameViewModel obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_List(obj.List, phase);
                    }
                }
            }
            private void Update_ViewModel_List(global::System.Collections.ObjectModel.ObservableCollection<global::System.String> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\GamePage.xaml line 221
                    if (!isobj3ItemsSourceDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj3, obj, null);
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Views\GamePage.xaml line 12
                {
                    this.ContentArea = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 5: // Views\GamePage.xaml line 131
                {
                    this.QuizGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 6: // Views\GamePage.xaml line 153
                {
                    this.ScreenshotGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 7: // Views\GamePage.xaml line 175
                {
                    this.SilhouetteGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 8: // Views\GamePage.xaml line 197
                {
                    this.SologameGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 9: // Views\GamePage.xaml line 206
                {
                    this.sologameChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 10: // Views\GamePage.xaml line 214
                {
                    this.SologameChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 11: // Views\GamePage.xaml line 184
                {
                    this.silhouetteChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 12: // Views\GamePage.xaml line 192
                {
                    this.SilhouetteChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 13: // Views\GamePage.xaml line 162
                {
                    this.screenshotChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 14: // Views\GamePage.xaml line 170
                {
                    this.ScreenshotChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 15: // Views\GamePage.xaml line 140
                {
                    this.quizChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 16: // Views\GamePage.xaml line 148
                {
                    this.QuizChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 17: // Views\GamePage.xaml line 39
                {
                    this.AudienceGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 18: // Views\GamePage.xaml line 60
                {
                    this.CrewGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 19: // Views\GamePage.xaml line 81
                {
                    this.MultipleChoiceGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 20: // Views\GamePage.xaml line 102
                {
                    this.MusicGlow = (global::Microsoft.Toolkit.Uwp.UI.Controls.DropShadowPanel)(target);
                }
                break;
            case 21: // Views\GamePage.xaml line 110
                {
                    this.musicChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 22: // Views\GamePage.xaml line 118
                {
                    this.MusicChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 23: // Views\GamePage.xaml line 89
                {
                    this.multipleChoiceChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 24: // Views\GamePage.xaml line 97
                {
                    this.MultipleChoiceChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 25: // Views\GamePage.xaml line 68
                {
                    this.crewChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 26: // Views\GamePage.xaml line 76
                {
                    this.CrewChallengeHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 27: // Views\GamePage.xaml line 47
                {
                    this.audienceChallenge = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 28: // Views\GamePage.xaml line 55
                {
                    this.AudienceHighLight = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // Views\GamePage.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    GamePage_obj1_Bindings bindings = new GamePage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Windows.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

