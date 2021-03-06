using System;
using MonoBrickFirmware.Display;
using MonoBrickFirmware.Display.Dialogs;

namespace MonoBrickFirmware.Display.Menus
{
	public class  MenuItemWithCharacterInput : IMenuItem
	{
		private string subject;
		private string dialogTitle;
		private Lcd lcd;
		private UserInput.Buttons btns;
		private const int lineSize = 2;
		private const int edgeSize = 2;
		private bool hide;
		public Action<Dialogs.CharacterDialog> OnShowDialog = delegate {};
		public Action<string> OnDialogExit = delegate {};
		public  MenuItemWithCharacterInput (Lcd lcd, UserInput.Buttons btns, string subject, string dialogTitle, string startText, bool hideInput = false){
			this.dialogTitle = dialogTitle; 
			this.subject = subject;
			this.Text = startText;
			this.lcd = lcd;
			this.btns = btns;
			this.hide = hideInput;
		}
		public bool EnterAction ()
		{
			var dialog = new Dialogs.CharacterDialog(lcd,btns,dialogTitle);
			dialog.OnShow += delegate{this.OnShowDialog(dialog);};
			dialog.OnExit += delegate{Text = dialog.GetUserInput();this.OnDialogExit(Text);};
			dialog.Show();
			return false;
		}
		public bool LeftAction (){return false;}
		public bool RightAction(){return false;}
		public void Draw (Font f, Rectangle r, bool color)
		{
			string showTextString;
			int totalWidth = r.P2.X - r.P1.X;
			int subjectWidth = (int)(f.TextSize (subject + "  ").X);
			int textValueWidth = totalWidth - subjectWidth;
			Rectangle textRect = new Rectangle (new Point (r.P1.X + subjectWidth, r.P1.Y), r.P2);
			Rectangle subjectRect = new Rectangle (r.P1, new Point (r.P2.X - textValueWidth, r.P2.Y));
			if ((int)(f.TextSize (Text).X) < textValueWidth) {
				showTextString = Text;
				if (hide) {
					showTextString = new string ('*', showTextString.Length); 
				}	
			} else {
				
				showTextString = "";
				for (int i = 0; i < Text.Length; i++) {
					if (f.TextSize (showTextString + this.Text [i] + "...").X < textValueWidth) {
						showTextString = showTextString + Text [i];
					} else {
						break;
					} 
				}
				if (hide) 
				{
					showTextString = new string ('*', showTextString.Length); 
				} 
				else 
				{
					showTextString = showTextString + "...";
				}
			}
			lcd.WriteTextBox (f, subjectRect,subject + "  ", color);
			lcd.WriteTextBox(f,textRect,showTextString,color,Lcd.Alignment.Right);
		}

		public string Text{get;private set;}
	}
}

