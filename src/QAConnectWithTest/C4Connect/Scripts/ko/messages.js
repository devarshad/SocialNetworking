var MessageType = {
    /// <summary>
    /// Indicates error occured while in the execution in the program.
    /// In red color.
    /// </summary>
    "Error": 1,

    /// <summary>
    /// Indicates information is displayed to the user.
    /// In blue color.
    /// </summary>
    "Information": 2,

    /// <summary>
    /// Indicates a task completion success.
    /// In green color.
    /// </summary>
    "Success": 3,

    /// <summary>
    /// Indicates a system or program warning.
    /// In orange color.
    /// </summary>
    "Warning": 4,

    /// <summary>
    /// Indicates progress bar.
    /// In yellow color.
    /// </summary>
    "Progress": 5
};

function Message(data) {
    var self = this;
    data = data || {};

    // Persisted properties
    self.Id = data.Id;
    self.Title = data.Title;
    self.IsDismissible = data.IsDismissible || true;
    self.Per = ko.observable('0');
    switch (data.Type) {
        case MessageType.Error:
            self.Type = "danger";
            break;
        case MessageType.Information:
            self.Type = "info";
            break;
        case MessageType.Success:
            self.Type = "success";
            break;
        case MessageType.Warning:
            self.Type = "warning";
            break;
        case MessageType.Progress:
            self.Type = "progress";
            break;
        default:
            self.Type = "danger";
            break;
    }
    self.Message = ko.observable(data.Message || "");
    self.percentComplete = ko.computed(function () {
        return this.Per();
    }, self);


    self.updateMessage = function (data) {
        self.Per(data.Per);
    }

    self.error = ko.observable();
}

function VM_Message() {
    var self = this;
    self.messages = ko.observableArray();
    self.count = ko.observable(0);
    self.loadMessages = function (data) {
        var mappedMessages = $.map(data, function (item) {
            item.Id = self.count(self.count() + 1);
            return new Message(item);
        });
        self.messages(self.messages().concat(mappedMessages));
    }

    self.newMessage = function (data) {
        self.messages.remove(function (item) {
            return item.Title == data.Title;
        });
        data.Id = self.count(self.count() + 1);
        var _message = new Message(data);
        self.messages.splice(0, 0, _message);
        return _message;
    }

    self.removeMessage = function (data) {
        //if (this)
        //    data = this;
        self.messages.remove(function (item) {
            return item.Id == data.Id;
        });
        self.count(self.count() - 1);
    }

    self.clearAll = function () {
        self.count(0);
        self.message([]);
    }

}

ko.bindingHandlers.progress = {
    init: function (element, valueAccessor) {
        $(element).css({
            'width': '0%'
        });
    },
    update: function (element, valueAccessor) {
        var val = ko.utils.unwrapObservable(valueAccessor());
        $(element).css('width', parseFloat(val) + '%');
    }
};