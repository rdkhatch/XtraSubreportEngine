using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions.Support;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRReportController : IReportController, IDisposable
    {
        private readonly XtraReport _view;
        private readonly XRRuntimeActionFacade _injectedFacade;

        private ScopedXRSubscriber _subscriber;
        public XRReportController(XtraReport view, XRRuntimeActionFacade injectedFacade = null)
        {
            _view = view;
            _injectedFacade = injectedFacade;
            GlobalXRSubscriber.Init();
        }

        protected virtual IEnumerable<IReportRuntimeAction> OnGetDefautActions()
        {
            yield return new PassSubreportDataSourceRuntimeAction();
        }

        protected virtual void OnRegisterAdditionalActions()
        {
        }

        private List<IReportRuntimeAction> _toDos;

        protected void RegisterFor<T>(Action<T> toDo) where T : XRControl
        {
            var action = ReportRuntimeAction<T>.WithNoPredicate(toDo);
            _toDos.Add(action);
        }

        public MyReportBase Print(Action<XtraReport> printAction)
        {
            var actions = OnGetDefautActions();
            var defaultFacade = new XRRuntimeActionFacade(actions.ToArray());

            _toDos = new List<IReportRuntimeAction>();
            OnRegisterAdditionalActions();
            var additionalActionsFacade = new XRRuntimeActionFacade(_toDos.ToArray());

            var newView = _view.ConvertReportToMyReportBase();
            newView.RuntimeRootReportHashCode = newView.GetHashCode();
            _subscriber = new ScopedXRSubscriber(newView.RuntimeRootReportHashCode, c =>
                                                                           {
                                                                               defaultFacade.AttemptActionsOnControl(c);
                                                                               additionalActionsFacade.AttemptActionsOnControl(c);

                                                                               if (_injectedFacade != null)
                                                                                   _injectedFacade.AttemptActionsOnControl(c);
                                                                           });
            printAction(newView);
            return newView;
        }

        public void Dispose()
        {
            _subscriber.Dispose();
        }
    }
}