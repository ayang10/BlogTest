using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogTest.Models
{


    public class Post
    {

        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Categories = new HashSet<Category>();
        }
        public int Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
        public string Title { get; set; }


        [AllowHtml]
        public string BodyText { get; set; }

        public string MediaUrl { get; set; }

        public bool Published { get; set; }
        public int CategoryId { get; set; }

        //joined table //add virtual for lazy loading
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        private int BodyTextLimit = 300;


        public string BodyTextTrimmed
        {
            get
            {
                if (this.BodyText.Length > this.BodyTextLimit)
                    return this.BodyText.Substring(0, this.BodyTextLimit) + "...";
                else
                    return this.BodyText;
            }
        }
        

        public static class ImageUploadValidator
        {
            public static bool IsWebFriendlyImage(HttpPostedFileBase fileUpload)
            {
                //check for actual object
                if (fileUpload == null)
                    return false;

                //check size - file must be  less than 2 MB and greater than 1 KB
                if (fileUpload.ContentLength > 2 * 1024 * 1024)
                    return false;

                try
                {
                    using (var img = Image.FromStream(fileUpload.InputStream))
                    {
                        return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                               ImageFormat.Png.Equals(img.RawFormat) ||
                               ImageFormat.Gif.Equals(img.RawFormat);
                    }
                }
                
                catch
                {

                    return false;
                }
            }
    }
        



    }

    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //joined table //add virtual for lazy loading
        public virtual ICollection<Post> Posts { get; set; }

    }


    public class Comment {
        public Comment()
        {
            this.Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; } //author
        public string EditorId { get; set; }//editior 
        [AllowHtml]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int ParentCommentId { get; set; }
        public bool MarkForDeletion { get; set; }

        public Post Post { get; set; }
        public ApplicationUser Author { get; set; }
        public ApplicationUser Editor { get; set; }
        public Comment ParentComment { get; set; }

        //each comments under another comments //add virtual for lazy loading
        public virtual ICollection<Comment> Comments { get; set; }
    }


    




}

    
