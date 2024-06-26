using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary
{
    public class TextResponseDto
    {
        public string Text {  get; set; }

        public TextResponseDto() 
        {
            Text = string.Empty;
        }

        public TextResponseDto(string text) 
        {
            Text = text;
        }
    }
}
