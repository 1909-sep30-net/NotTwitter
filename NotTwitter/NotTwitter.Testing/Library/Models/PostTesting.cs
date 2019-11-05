using NotTwitter.Library.Models;
using System;
using Xunit;

namespace NotTwitter.Testing.Library.Models
{
	public class PostTesting
	{
		[Theory]
		[InlineData(null)]
		public void IsPostNull(string content) 
		{
			Post post = new Post();
			Assert.Throws<ArgumentNullException>(() => post.Content = content);
		}

		[Theory]
		[InlineData("")]
		public void IsPostEmpty(string content)
		{
			Post post = new Post();
			Assert.Throws<ArgumentOutOfRangeException>(()=>post.Content = content);
		}

		[Fact]
		public void IsPostTooLong()
		{
		Post post = new Post();

            // 282 Character string
            string longString = "Lorem ipsum dolor sit amet consectetur adipiscing elit," +
                " mollis lectus est tempus auctor malesuada, nam sociis dignissim habitant" +
                " nec varius litora, vestibulum sem vel odio etiam. Arcu dignissim quis sem" +
                " tempor ac ornare praesent eget nascetur, et malessuada class habitasse egestas.";

            Assert.Throws<ArgumentOutOfRangeException>(() => post.Content = longString);			
		}

        [Fact]
        public void ShorterPostStoresCorrectly()
        {
            Post post = new Post();

            // 282 Character string
            string longString = "Lorem ipsum dolor sit amet consectetur adipiscing elit," +
                " mollis lectus est tempus auctor malesuada, nam sociis dignissim habitant" +
                " nec varius litora.";

            post.Content = longString;

            Assert.Equal(longString, post.Content);
        }
    }
}
