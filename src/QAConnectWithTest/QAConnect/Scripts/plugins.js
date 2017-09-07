/**
 * Central AJAX caller gateway.
 * @author Mohmmad Arshad Khan
 *
 */
var baseUrl = 'http://localhost:57715/';
(function ($) {
    $.myAjax = function (options) {
        var tokenKey = 'accessToken';
        var _message = {};
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        // This is the easiest way to have default options.
        var _settings = $.extend({
            // These are the defaults.
            type: 'GET',
            url: baseUrl,
            headers: headers,
            async: true,
            data: {},
            dataType: 'json',
            cache: true,
            processData: true,
            contentType: 'application/json',
            showProgress: false,
        }, options);

        // ajax the collection based on the _settings variable.

        $.ajax({
            type: _settings.type, //mandatory field
            url: _settings.url, //mandatory
            headers: _settings.headers,
            async: _settings.async,
            data: _settings.data,
            dataType: _settings.dataType,
            cache: _settings.cache,
            contentType: _settings.contentType,
            processData: _settings.processData,
            xhr: function () {
                var xhr = new XMLHttpRequest();
                if (_settings.showProgress) {
                    //Upload progress
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
                            _message.updateMessage({ 'Per': percentComplete });
                            if (_settings.progress)
                                _settings.progress(percentComplete);
                        }
                    }, false);
                    //Download progress
                    xhr.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
                            _message.updateMessage({ 'Per': percentComplete });
                            if (_settings.progress)
                                _settings.progress(percentComplete);
                        }
                    }, false);
                }
                return xhr;
            },
            //beforeSend: function (jqXHR, _settings) {
            beforeSend: function (jqXHR, settings) {
                if (_settings.showProgress) {
                    _message = vmMessage.newMessage({ 'Title': 'Processing request...', 'Type': MessageType.Progress });
                }
            },
            success: function (data, textStatus, jqXHR) {
                //Handle success status and do more
                if (_settings.success)
                    _settings.success(data); //Invoke success callback method.   
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Handle error status and do more
                vmMessage.removeMessage(_message);
                var error = "";
                if (jqXHR.status === 0) {
                    error = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    error = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    error = 'Internal Server Error [500].';
                } else if (errorThrown === 'parsererror') {
                    error = 'Requested JSON parse failed.';
                } else if (errorThrown === 'timeout') {
                    error = 'Time out error.';
                } else if (errorThrown === 'abort') {
                    error = 'Request aborted.';
                } else {
                    error = 'Uncaught Error.';
                }
                try {
                    $.each(eval(jqXHR.responseJSON.Message), function (i, o) {
                        $.each(o.Errors, function (_i, _o) {
                            error += '\n' + _o;
                        });
                    });
                } catch (e) {

                }
                vmMessage.newMessage({ 'Title': 'Fix following error(\'s):-', 'Type': MessageType.Error, 'Message': error });
                if (_settings.error)
                    _settings.error(jqXHR, textStatus, errorThrown)
            },
            complete: function (jqXHR, textStatus, errorThrown) {
                vmMessage.removeMessage(_message);
            }
        });
    };

}(jQuery));

/**
 * Check file against target type
 * @author Mohmmad Arshad Khan
 *
 */

checkFile = function (file, targetType) {
    var fileExtensions = [0, 0, ['gif', 'png', 'jpg', 'jpeg'], 0, 0, ['mp3'], ['mp4', 'flv', 'avi', 'wmv']];
    var fileSize = [0, 0, 2, 0, 0, 5, 5];
    if ($.inArray(file.name.split('.').pop(), fileExtensions[targetType]) == -1) {
        return 'Invalid file type.\nPlease select ' + fileExtensions[targetType] + ' files.';
    };

    if (file.size / 1024 / 1024 > fileSize[targetType]) {
        return 'File too large.\nPlease select file upto ' + fileSize[targetType] + ' MB.';
    }

    return true;
}

/**
 *Check post validation
 * @author Mohammad ARshad Khan 
 */
checkPost = function (post) {
    var error = '';
    if (post.Message == '' || post.Message == null || post.Message == undefined) {
        error += 'Message is required.\n'
    }
    if (post.PostType == 2 || post.PostType == 4 || post.PostType == 5 || post.PostType == 6) {
        if (post.Link == '' || post.Link == null || post.Link == undefined) {
            error += 'Attachment is required.\n'
        }
    }
    if (error.length > 0) {
        return error;
    }
    else
        return true;
}