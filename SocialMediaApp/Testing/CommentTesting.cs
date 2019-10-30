using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Testing
{
	public class CommentTesting
	{

		[Theory]
		[InlineData(null)]
		public void NullCommentThrowsArgNullException(string content)
		{
			Library.Models.Comment comment = new Library.Models.Comment();
			Assert.Throws<ArgumentNullException>(() => comment.Content = content);
		}

		[Theory]
		[InlineData("")]
		public void EmptyCommentThrowsArgOORException(string content)
		{
			Library.Models.Comment comment = new Library.Models.Comment();
			Assert.Throws<ArgumentOutOfRangeException>(() => comment.Content = content);
		}

		[Fact]
		public void LongCommentThrowsArgOORException()
		{
			Library.Models.Comment comment = new Library.Models.Comment();

            // 282 Character string
            string longString = "Lorem ipsum dolor sit amet consectetur adipiscing elit," +
                " mollis lectus est tempus auctor malesuada, nam sociis dignissim habitant" +
                " nec varius litora, vestibulum sem vel odio etiam. Arcu dignissim quis sem" +
                " tempor ac ornare praesent eget nascetur, et malessuada class habitasse egestas.";

			Assert.Throws<ArgumentOutOfRangeException>(() => comment.Content = longString);
		}
	}
}
