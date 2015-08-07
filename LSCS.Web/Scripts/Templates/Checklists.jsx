

var Checklist = React.createClass({
    handleClick: function(e) {
        e.preventDefault();
        this.props.onDelete(this.props.index, this.props.data.Id);
    },
    render: function() {
        var showURL = "http://localhost:49177/checklists/" + this.props.data.Id;
        var editUrl = "http://localhost:49177/checklists/edit/" + this.props.data.Id;
        return (
            <tr>
                <td><a href={showURL}>{this.props.data.Title}</a></td>
                <td>{this.props.data.SurveyLocation.LandDistrict.Name}</td>
                <td>{this.props.data.Description}</td>
                <td>{this.props.data.FileNumber}</td>
                <td><a href={editUrl}>Edit</a> <a href="#" onClick={this.handleClick}>Delete</a></td>
            </tr>
        );
    }
}); 

var ChecklistList = React.createClass({
    loadChecklistsFromServer: function() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function() {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    componentDidMount: function() {
        this.loadChecklistsFromServer();
        window.setInterval(this.loadChecklistsFromServer, this.props.pollInterval);
    },
    getInitialState: function() {
        return {data: []};
    },
    deleteChecklist: function(index, id) {
        var deleteUrl = "http://localhost:1059/api/checklists/" + id;
        var checklists = this.state.data;
        var component = this;

        $.ajax({
            method: "DELETE",
            url: deleteUrl,
            success: function(msg) {
                component.setState({ data: checklists.splice(index, 1) })
            }
        })
        .fail(function (jqXHR, status, error) {
            alert("Failed to delete checklist: " + status);
        })
    },
    render: function() {
        var deleteFunction = this.deleteChecklist;
        var checklistNodes = this.state.data.map(function (checklist, index) {
            return (
                <Checklist data={checklist} index={index} key={index} onDelete={deleteFunction} />
            );
        });
        return (
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Location</th>
                        <th>Description</th>
                        <th>File No.</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {checklistNodes}
                </tbody>
            </table>     
        );
    }
});

React.render(
    <ChecklistList url="http://localhost:1059/api/checklists" pollInterval={2000} />,
    document.getElementById('checklist-list')
);