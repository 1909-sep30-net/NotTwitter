using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Testing
{
	public class PostTesting
	{
		[Theory]
		[InlineData(null)]
		public void IsPostNull(string content) 
		{
			Library.Models.Post post = new Library.Models.Post();
			Assert.Throws<ArgumentNullException>(() => post.Content = content);
		}

		[Theory]
		[InlineData("")]
		public void IsPostEmpty(string content)
		{
			Library.Models.Post post = new Library.Models.Post();
			Assert.Throws<ArgumentOutOfRangeException>(()=>post.Content = content);
		}

		[Fact]
		public void IsPostTooLong()
		{
			Library.Models.Post post = new Library.Models.Post();

            // 282 Character string
            string longString = "Lorem ipsum dolor sit amet consectetur adipiscing elit," +
                " mollis lectus est tempus auctor malesuada, nam sociis dignissim habitant" +
                " nec varius litora, vestibulum sem vel odio etiam. Arcu dignissim quis sem" +
                " tempor ac ornare praesent eget nascetur, et malessuada class habitasse egestas.";

            Assert.Throws<ArgumentOutOfRangeException>(() => post.Content = longString);			
		}
	}
}
