using System.Web.Mvc;
using Autofac;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
	public class BindersModule : Module
	{
		private readonly ModelBinderDictionary binders;

		public BindersModule(ModelBinderDictionary binders)
		{
			this.binders = binders;
		}

		protected override void Load(ContainerBuilder builder)
		{
			if (!binders.ContainsKey(typeof(PageName))) binders.Add(typeof(PageName), new ImplicitAssignmentBinder());
			if (!binders.ContainsKey(typeof(int[]))) binders.Add(typeof(int[]), new ArrayBinder());
			if (!binders.ContainsKey(typeof(FileUpload))) binders.Add(typeof(FileUpload), new UploadBinder());
			if (!binders.ContainsKey(typeof(EntryRevision))) binders.Add(typeof(EntryRevision), new EntryRevisionBinder());
		}
	}
}
