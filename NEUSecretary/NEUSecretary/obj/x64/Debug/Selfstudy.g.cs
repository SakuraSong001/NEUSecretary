﻿#pragma checksum "D:\code\NEUSecretary\NEUSecretary\NEUSecretary\Selfstudy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4DFEA1968088188F3B36AA7ED502A67B"
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
                    this.SelfStudyRelativePanel = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                }
                break;
            case 2:
                {
                    this.SstudyGridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 3:
                {
                    this.SearchBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 120 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchBtn).Click += this.SearchBtn_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.RoomComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 57 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.RoomComboBox).SelectionChanged += this.RoomComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 5:
                {
                    this.SelfStudy_DatePicker = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                }
                break;
            case 6:
                {
                    this.ResultWeek = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.WeekdayComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 90 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.WeekdayComboBox).SelectionChanged += this.WeekdayComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 8:
                {
                    this.TimeComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 103 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.TimeComboBox).SelectionChanged += this.TimeComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 9:
                {
                    this.YifuHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 10:
                {
                    this.DachengHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 11:
                {
                    this.JidianHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 12:
                {
                    this.JianzhuHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 13:
                {
                    this.XinxiHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 14:
                {
                    this.ShengmingHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 15:
                {
                    this.WenguanHouse = (global::Windows.UI.Xaml.Controls.ComboBoxItem)(target);
                }
                break;
            case 16:
                {
                    this.TermChooseButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 31 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.TermChooseButton).Click += this.TermChooseButton_Click;
                    #line default
                }
                break;
            case 17:
                {
                    this.TermBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 39 "..\..\..\Selfstudy.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.TermBox).SelectionChanged += this.TermBox_SelectionChanged;
                    #line default
                }
                break;
            case 18:
                {
                    this.TermTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

