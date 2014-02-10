(function ($) {
  $.extend($.fn, {
    selectUser:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        windowid: '_window',
        memberid: 0,
        treeid: '_tree',
        url: '',
        selectid: '_select',
        onSelected: null,
        filterChar: 'd_',
        checkboxes: {
          checkChildren: true
        }
      }, setting);
      var that = this;
      var id = that.attr('id');
      var isInited = false;
      var wd = $("#" + id + ps.windowid).data("kendoWindow");
      $('#' + id + ps.selectid).on('click', ps.onSelected)
      var treeview = null;
      var singValue = null;
      function binding() {
        bindData().done(function (res) {
          treeview.dataSource.bind('change', getValue)
          wd.center().open();
          isInited = true;
        })
      }

      function hide() {
        wd.close();
      }

      function show() {
        if (isInited) {
          wd.center().open();
        } else {
          binding();
        }
      }

      function getValue() {
        if (ps.checkboxes) {
          var checkedNodes = [];
          checkedNodeIds(treeview.dataSource.view(), checkedNodes);
          if (checkedNodes.length > 0) {
            return checkedNodes.join(',');
          } else {
            return '';
          }
        } else {
          return singValue;
        }
      }

      function checkedNodeIds(nodes, checkedNodes) {
        for (var i = 0; i < nodes.length; i++) {
          if (nodes[i].checked) {
            if (ps.filterChar != '' && nodes[i].id.indexOf(ps.filterChar) > -1) {
            } else {
              checkedNodes.push(nodes[i].id);
            }
          }
          if (nodes[i].hasChildren) {
            checkedNodeIds(nodes[i].children.view(), checkedNodes);
          }
        }
      }

      function bindData() {
        var d = $.Deferred();
        $.get(ps.url, { ID: ps.memberid }, function (res) {
          treeview = $('#' + id + ps.treeid).kendoTreeView({
            checkboxes: ps.checkboxes,
            dataSource: res,
            select: onTreeViewSelect,
          }).data('kendoTreeView');
          d.resolve(true);
        })
        return d.promise();
      }

      function onTreeViewSelect(e) {
        var dataitem = this.dataItem(e.node);
        if (dataitem.id.indexOf(ps.filterChar) == -1) {
          singValue = dataitem;
        } else {
          singValue = null;
        }
      }

      return {
        window: wd,
        treeview: treeview,
        getValue: getValue,
        show: show,
        hide: hide,
        isInited: isInited
      }
    }
  })
})(jQuery)