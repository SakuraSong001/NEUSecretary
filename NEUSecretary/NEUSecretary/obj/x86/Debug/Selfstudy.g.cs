﻿#pragma checksum "D:\V.S.15\代码\NEUSecretary\NEUSecretary\NEUSecretary\Selfstudy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F4C688A5F3F21700C5729A8A086FD259"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NEUSecretary
{
    partial class Selfstudy : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.TermComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 27 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.TermComboBox).SelectionChanged += this.TermComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.WeekdayComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 38 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.WeekdayComboBox).SelectionChanged += this.WeekdayComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 3:
                {
                    this.TimeComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 51 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.TimeComboBox).SelectionChanged += this.TimeComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 4:
                {
                    this.RoomComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 63 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.RoomComboBox).SelectionChanged += this.RoomComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 5:
                {
                    this.ResultWeek = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6:
                {
                    this.SearchBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 76 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchBtn).Click += this.SearchBtn_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.room11 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.room12 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.room21 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.room22 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

