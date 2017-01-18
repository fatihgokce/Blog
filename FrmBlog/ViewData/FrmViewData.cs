using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;
namespace FrmBlog.ViewData
{
    public class FrmViewData
    {
        public Answer Answer { get; set; }
        public List<Answer> Answers { get; set; }
        public FrmViewData WithAnswer(Answer item)
        {
            this.Answer = item;
            return this;
        }
        public FrmViewData WithAnswers(List<Answer> items)
        {
            this.Answers = items;
            return this;
        }      
        public FrmViewData  WithQuestions(List<Question>liste)
        {
            this.Questions=liste;
            return this;
        }
        public Question Question { get; set; }
        public List<Question> Questions { get; set; }
        public FrmViewData WithQuestion(Question item)
        {
            this.Question = item;
            return this;
        }
        public QuestionTag QuestionTag { get; set; }
        public List<QuestionTag> QuestionTags { get; set; }
        public FrmViewData WithQuestionTag(QuestionTag item)
        {
            this.QuestionTag = item;
            return this;
        }
        public FrmViewData WithQuestionTags(List<QuestionTag> items)
        {
            this.QuestionTags = items;
            return this;
        }
        public Tag Tag { get; set; }
        public List<Tag> Tags { get; set; }
        public FrmViewData WithTag(Tag item)
        {
            this.Tag = item;
            return this;
        }
        public FrmViewData WithTags(List<Tag> items)
        {
            this.Tags = items;
            return this;
        }
        public QuestionVisit QuestionVisit { get; set; }
        public List<QuestionVisit> QuestionVisits { get; set; }
        public FrmViewData WithQuestionVisit(QuestionVisit item)
        {
            this.QuestionVisit = item;
            return this;
        }
        public FrmViewData WithQuestionVisits(List<QuestionVisit> items)
        {
            this.QuestionVisits = items;
            return this;
        }
        public User User { get; set; }
        public List<User> Users { get; set; }
        public FrmViewData WithUser(User item)
        {
            this.User = item;
            return this;
        }
        public FrmViewData WithUsers(List<User> items)
        {
            this.Users = items;
            return this;
        }
        public RegisterModel RegisterModel { get; set; }
        public FrmViewData WithRegisterModel(RegisterModel register)
        {
            this.RegisterModel = register;
            return this;
        }
        public PaginatedList Paging { get; set; }
        public FrmViewData WithPaging(PaginatedList paging)
        {
            this.Paging = paging;
            return this;
        }
    }
    public class FrmView
    {
        public static FrmViewData Data { get { return new FrmViewData(); } }
    }
}