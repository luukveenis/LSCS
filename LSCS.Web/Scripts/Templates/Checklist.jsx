$(function(){

    var Checklist = React.createClass({
        retrieveChecklistFromServer: function() {
            var xhr = new XMLHttpRequest();
            xhr.open('get', this.props.url, true);
            xhr.onload = function() {
                var data = JSON.parse(xhr.responseText);
                this.setState({data: data});
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
                        <div className="col-md-9">
                            <h4>Created on: {this.state.data.CreatedAt}</h4>
                            <h4>Last modified: {this.state.data.LastModified}</h4>
                            <h4>Land District: {this.state.data.SurveyLocation.LandDistrict.Name}</h4>
                            <br />
                            <h4>Description: {this.state.data.Description}</h4>
                        </div>
                        <div className="col-md-3">
                            <address>
                                {this.state.data.SurveyLocation.Address.AddressLine1}<br/>
                                {this.state.data.SurveyLocation.Address.City}, {this.state.data.SurveyLocation.Address.PostalCode}<br/>
                                {this.state.data.SurveyLocation.Address.StateProvince}, {this.state.data.SurveyLocation.Address.CountryRegion}<br/>
                            </address>
                        </div>
                    </div>
                    <br/>
                    <iframe id="forecast_embed" type="text/html" frameBorder="0" height="245" width="100%"
                        src={"http://forecast.io/embed/#lat=" + this.state.data.SurveyLocation.Coordinate.Latitude + "&lon=" + this.state.data.SurveyLocation.Coordinate.Longitude + "&name=" + this.state.data.SurveyLocation.LandDistrict.Name}>
                    </iframe>
                    <iframe src={"https://www.google.com/maps/embed/v1/place?q=" + this.state.data.SurveyLocation.Coordinate.Latitude + "%2C" + this.state.data.SurveyLocation.Coordinate.Longitude + "&key=AIzaSyCJ03ynONA_qVG4ILQ6F5Zlo4DB8YHdBb0" }
                        width="600" height="450" frameBorder="0" style={{ border:0 }} allowFullScreen></iframe>
                    <br/>
                    <ItemTable title="Section A: Plan Title" data={this.state.data.Items.slice(0,8)} tableNum="1"/>
                    <ItemTable title="Section B: Main Body of Plan" data={this.state.data.Items.slice(8,26)} tableNum="2"/>
                    <ItemTable title="Section C: Scenery" data={this.state.data.Items.slice(26,33)} tableNum="3"/>
                    <ItemTable title="Section D: Deposit Statement" data={this.state.data.Items.slice(33,35)} tableNum="4"/>
                    <ItemTable title="Section E: Integrated Survey Area" data={this.state.data.Items.slice(35,39)} tableNum="5"/>
                    <ItemTable title="Section F: Miscellaneous" data={this.state.data.Items.slice(39,43)} tableNum="6"/>
                    <ItemTable title="Section G: Electronic Plan" data={this.state.data.Items.slice(43,47)} tableNum="7"/>
                </div>
            );
        
        }
    });

    var ItemTable = React.createClass({
        render: function(){
            var tableNum = this.props.tableNum;
            var ItemTableRows = this.props.data.map(function(row, index){
                return (
                   <ItemTableRow data={row} key={index} tableNum={tableNum} rowNum={index}/> 
                );
            });
            return (
                <table className="table table-hover col-md-12">
                    <thead>
                        <tr>
                            <th className="col-md-10">{this.props.title}</th>
                            <th className="col-md-2">Status:</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ItemTableRows}
                    </tbody>
                </table>
            );
        }
    });

    var ItemTableRow = React.createClass({
        render: function() {
            var status = this.props.data.Status;
            var radioGroup = "status_"+this.props.tableNum+"_"+this.props.rowNum;
            return (
                <tr>
                    <td>{this.props.data.Text}</td>
                    <td><input type="radio" name={radioGroup} value="Unanswered" checked={status == "Unanswered"} readOnly/>Unanswered</td>
                    <td><input type="radio" name={radioGroup} value="Yes" checked={status == "Yes"} readOnly/>Yes</td>
                    <td><input type="radio" name={radioGroup} value="N/A" checked={status == "NotApplicable"} readOnly/>N/A</td>
                </tr>
            );
        }
    });

    var checklistId = $('#checklist-view').data('id')
    React.render(
        <Checklist url={"http://localhost:1059/api/checklists/" + checklistId} pollInterval={2000} />,
        document.getElementById('checklist-view')
    );
});