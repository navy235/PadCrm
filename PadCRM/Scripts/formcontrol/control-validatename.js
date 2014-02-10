(function ($) {
  $.extend($.fn, {
    validateName:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        elementId: '_area',
        ajaxUrl: '',
        value: ''
      }, setting);
      var that = this;
      var id = that.attr('id');
      var element = $('#' + id + ps.elementId);
      element.on('keydown', '#' + id, keydownHandler)
      function keydownHandler(e) {
        if (e.ctrlKey && e.keyCode == 86) {
          return false;
        }
        var currentValue = that.val().toLowerCase();
        if (currentValue.length > 2) {
          $.get(ps.ajaxUrl, {
            key: currentValue
          }, function (res) {
            if (res.length > 0) {
              createSelectList(res);
            }
          })
        }
      }

      function createSelectList(res) {
        if (that.droppanel[0]) {
          that.droppanel.empty();
        } else {
          var droppannel = '<div class="validate-pannel"></div>';
          $('body').append(droppannel);
          that.droppanel = droppannel;
        }
        $.each(res, function (index, item) {
          var option = '<div class="validate-item" data-index="' + index + '">'
            + item.name
            + '</div>';
          var option = $('<div/>').addClass('validate-item').data('value', item);
          that.droppanel.append(option);
        })
        that.droppanel.show();
        $(document).bind('mousedown', function (e) {
          if ($(e.target).hasClass('validate-panel')
            || $(e.target).parents('.dropdown-panel')[0]
            || $(e.target).hasClass('validate-container')
            || $(e.target).parents('.validate-container')) {
          } else {
            that.droppanel.hide();
            $(document).unbind('mousedown');
          }
        })
      }



      binding();
    }
  });

})(jQuery)