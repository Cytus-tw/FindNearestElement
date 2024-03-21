using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit2021_FindNearestElement
{

    /****************************************************************
    
    this practice handles finding the nearest element in a given point and direction.
    it is very useful while searching element and calculate the distance.
     
    feel free to apply this to any different purpose

    **************************************************************/


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FindNearestElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet element)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // first you need a view3d
            View3D v3d = doc.ActiveView as View3D; 

            //second you need a filter, in this case I put StructuralColums as example,
            //feel free to adjust to whatever you want or you are looking for.
            ElementCategoryFilter ECF = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);

            //third you need an intersector to store the element you found.
            //the target reference can be different type such as face, point or edge.
            //dont forget to put the view3d in it.
            ReferenceIntersector RefInt = new ReferenceIntersector(ECF, FindReferenceTarget.Face, v3d);

            //finally we need a reference with context to store the result, and convert to element at last.
            ReferenceWithContext RefWC = null;


            //to execute Find method, we need one position and one vector
            //both are XYZ class

            //feel free to apply to different values in different need.
            XYZ originalPos = new XYZ(0, 0, 0); // let's find element from (0,0,0) golbal position
            XYZ direction = new XYZ(0, 0, 1); // and search with the vector (0,0,1)

            //the result will be store in RefWC by RefInt

            RefWC = RefInt.FindNearest(originalPos, direction);

            //care that in revit there are many weird elements that may lead to an error
            //and don't forget it is possible that we can't find any of them.

            if(RefWC == null)
            {
                //do something
            }

            //At last , convert the final result to element class so that we can work on different purpose, such as parameters

            Element theFinalElement = doc.GetElement(RefWC.GetReference());


            return Result.Succeeded;
        }

    }
}
