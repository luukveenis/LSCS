var Checklist = React.createClass({
    render: function() {
        return (
            <tr>
                <td>{this.props.data.Title}</td>
                <td>{this.props.data.Description}</td>
                <td>{this.props.data.FileNumber}</td>
                <td>{this.props.data.Location}</td>
            </tr>
        );
    }
});

var ChecklistList = React.createClass({
    render: function() {
        var checklistNodes = this.props.data.map(function (checklist) {
            return (
                <Checklist data={checklist} />
            );
        });
        return (
            <table>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Description</th>
                        <th>File No.</th>
                        <th>Location</th>
                    </tr>
                </thead>
                <tbody>
                    {checklistNodes}
                </tbody>
            </table>
        );
    }
});

var data = [ { Title: "Test Title", Description: "Some stuff in here", FileNumber: 2053, Location: "The Moon" } ]

React.render(
    <ChecklistList data={data} />,
    document.getElementById('checklist-list')
);