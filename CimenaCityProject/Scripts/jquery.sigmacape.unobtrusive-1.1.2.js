function debug(message) {
    if (window.console && window.console.log) {
        window.console.log(message);
    }
};

(function ($) {
    //
    // Extends on the jQuery ajax method. Automically shows a loading element after a specified delay.
    //
    $.asyncRequest = function (options) {
        var loadingThread;

        var settings = $.extend({}, options, {
            loadingElement: options.loadingElement,
            delayLoader: options.delayLoader || true,
            delayTime: options.delayTime || 300,
            beforeSend: function () {
                //
                // Context: settings
                //
                var result = options.beforeSend && options.beforeSend();
                if (result !== false) {
                    if (settings.delayLoader) {
                        loadingThread = setTimeout(settings.loadingShow, settings.delayTime);
                    } else {
                        settings.loadingShow();
                    }
                }
            },
            complete: function () {
                debug("async request: complete");
                
                //
                // Context: settings
                //
                clearTimeout(loadingThread);

                var result = options.complete && options.complete();
                if (result !== false) {
                    settings.loadingHide();
                }
            },
            loadingShow: function () {
                //
                // Context: settings
                //
                var result = options.loadingShow && options.loadingShow();
                if (result !== false) {
                    $(settings.loadingElement).show();
                }
            },
            loadingHide: function () {
                //
                // Context: settings
                //
                var result = options.loadingHide && options.loadingHide();
                if (result !== false) {
                    $(settings.loadingElement).hide();
                }
            }
        });

        return $.ajax(settings);
    };

    //
    // Extends on the async request method. Optionally allows calling of attribute and user setting defined methods.
    //
    $.fn.asyncRequest = function (options) {
        function asyncRequest(element, options) {

            function getFunction(code) {
                var fn = window,
                    parts = (code || '').split('.');

                while (fn && parts.length) {
                    fn = fn[parts.shift()];
                }

                if (typeof (fn) === 'function') {
                    return fn;
                }

                return Function.constructor.apply(null);
            }

            function onSuccess(data) {
                var mode = (element.getAttribute('data-ajax-mode') || '').toUpperCase();

                $(element.getAttribute('data-ajax-update')).each(function (i, update) {
                    switch (mode) {
                        case 'BEFORE':
                            var top = update.firstChild;

                            $('<div />').html(data).contents().each(function () {
                                update.insertBefore(this, top);
                            });
                            break;
                        case 'AFTER':
                            $("<div />").html(data).contents().each(function () {
                                update.appendChild(this);
                            });
                            break;
                        default:
                            $(update).html(data);
                            break;
                    }
                });
            }

            function post() {
                var confirm = element.getAttribute('data-ajax-confirm');
                if (confirm && window.confirm(confirm) === false) {
                    return undefined;
                }

                var settings = $.extend({}, {
                    type: element.getAttribute('data-ajax-method') || 'GET',
                    url: element.getAttribute('data-ajax-url'),
                    loadingElement: element.getAttribute('data-ajax-loading')
                }, options);

                var mergedOptions = $.extend({}, settings, {
                    beforeSend: function () {
                        debug("async request: sending");
                        
                        //
                        // Context: mergedOptions
                        //
                        if (settings.beforeSend) {
                            settings.beforeSend();
                        }

                        return getFunction(element.getAttribute('data-ajax-begin')).apply(element);
                    },
                    complete: function () {
                        //
                        // Context: mergedOptions
                        //
                        if (settings.complete) {
                            settings.complete();
                        }

                        return getFunction(element.getAttribute('data-ajax-complete')).apply(element);
                    },
                    success: function (data) {
                        debug("async request: success");
                        
                        //
                        // Context: mergedOptions
                        //
                        if (settings.success) {
                            settings.success(data);
                        }

                        onSuccess(data);
                        return getFunction(element.getAttribute('data-ajax-success')).apply(element, [data]);
                    },
                    error: function (data) {
                        debug("async request: error");
                        
                        //
                        // Context: mergedOptions
                        //
                        if (settings.error) {
                            settings.error(data);
                        }

                        return getFunction(element.getAttribute('data-ajax-failure')).apply(element, [data]);
                    },
                    loadingShow: function () {
                        //
                        // Context: mergedOptions
                        //
                        if (settings.loadingShow) {
                            settings.loadingShow();
                        }

                        return getFunction(element.getAttribute('data-ajax-loading-show')).apply(settings.loadingElement || element);
                    }
                });

                return $.asyncRequest(mergedOptions);
            }

            return post();
        }

        return this.each(function () {
            asyncRequest(this, options);
        });
    };

    $(document).on('click', 'a[data-ajax=true]', function (e) {
        e.preventDefault();
        debug("ajax link click: " + this.href);
        
        $(this).asyncRequest({
            url: this.href
        });
    });

    $(document).on('submit', 'form[data-ajax=true]', function (e) {
        e.preventDefault();
        debug("ajax form submit: " + this.action);
        
        var form = $(this);
        if (form.valid()) {
            $('.validation-summary-errors').remove();

            form.asyncRequest({
                type: this.method || 'GET',
                url: this.action,
                data: form.serialize()
            });
        }
    });
})(jQuery, window, document);