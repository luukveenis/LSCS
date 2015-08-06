var Checklist = React.createClass({
    render: function() {
        editUrl = "http://localhost:49177/checklists/edit/" + this.props.data.Id;
        deleteUrl = "http://localhost:1059/api/checklists/" + this.props.data.Id;
        return (
            <tr>
                <td>{this.props.data.Title}</td>
                <td>{this.props.data.SurveyLocation.LandDistrict.Name}</td>
                <td>{this.props.data.Description}</td>
                <td>{this.props.data.FileNumber}</td>
                <td><a href={editUrl}>Edit</a> <a href={deleteUrl} className="delete-checklist-link">Delete</a></td>
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
    render: function() {
        var checklistNodes = this.state.data.map(function (checklist) {
            return (
                <Checklist data={checklist} />
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