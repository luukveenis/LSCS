$(function(){

    var ItemTableRow = React.createClass({
        render: function() {
            return (
                <tr>
                    <td>this.props.data.Text</td>
                    <td>this.props.data.Status</td>
                </tr>
            );
        }
    });

    var ItemTable = React.createClass({
        render: function(){
            var ItemTableRows = this.props.data.map(function(row, index){
                return (
                   <ItemTableRow data={row} key={index} /> 
                );
            });
            return (
                <table className="table table-hover">
                    <thead>
                        <tr>
                            <th>{this.props.title}</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ItemTableRows}
                    </tbody>
                </table>
            );
        }
    })

    var Checklist = React.createClass({
        retrieveChecklistFromServer: function() {
            var xhr = new XMLHttpRequest();
            xhr.open('get', this.props.url, true);
            xhr.onload = function() {
                var data = JSON.parse(xhr.responseText);
                this.setState({data: data[0]}); //Needs to be set to the single checklist to remove the [0] hardcoded indexing necessary on props
                debugger;
            }.bind(this);
            xhr.send();
        },
        componentDidMount: function() {
            this.retrieveChecklistFromServer();
            window.setInterval(this.retrieveChecklistFromServer, this.props.pollInterval);
        },
        getInitialState: function() {
            return {data: []};
        },
        render: function() {           
            //Need to format date fields
            return (this.state.data.length == 0) ? <div></div> : (
                <div>
                    <div className="checklist-hdr">
                        <h2 className="checklist-hdr-title">#{this.state.data.FileNumber} - {this.state.data.Title}</h2>
                        <div className="col-md-10">
                            <h4>Created on: {this.state.data.CreatedAt}</h4>
                            <h4>Last modified: {this.state.data.LastModified}</h4>
                            <h4>Land District: {this.state.data.SurveyLocation.LandDistrict.Name}</h4>
                        </div>
                        <div className="col-md-2">
                            <address>
                                {this.state.data.SurveyLocation.Address.AddressLine1}<br/>
                                {this.state.data.SurveyLocation.Address.City}, {this.state.data.SurveyLocation.Address.PostalCode}<br/>
                                {this.state.data.SurveyLocation.Address.StateProvince}, {this.state.data.SurveyLocation.Address.CountryRegion}<br/>
                            </address>
                        </div>
                    </div>
                    <br/>
                    <div className="checklist-descr col-md-12">
                        <p>{this.state.data.Description}</p>
                    </div>
                
                    <ItemTable title="Section A: Plan Title" data={this.state.data.Items} />
                </div>
            );
        
        }
    });

    var checklistId = $('#checklist-view').data('id')
    React.render(
        <Checklist url={"http://localhost:1059/api/checklists/" + checklistId} pollInterval={2000} />,
        document.getElementById('checklist-view')
    );
});