using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

public static class ViewRenderer
{
    public static string RenderViewToString(Controller controller, string viewName, object model)
    {
        controller.ViewData.Model = model;

        using var writer = new StringWriter();
        var engine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        var viewResult = engine?.FindView(controller.ControllerContext, viewName, false);

        if (viewResult?.View == null)
            throw new ArgumentNullException($"View {viewName} not found.");

        var viewContext = new ViewContext(
            controller.ControllerContext,
            viewResult.View,
            controller.ViewData,
            controller.TempData,
            writer,
            new HtmlHelperOptions()
        );

        viewResult.View.RenderAsync(viewContext).Wait();
        return writer.GetStringBuilder().ToString();
    }
}
