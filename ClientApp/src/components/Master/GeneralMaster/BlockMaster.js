import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import SectorDrop from '../../Util/SectorDrop.js';

export default class BlockMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            BlockList: [],
            loading: true,
            FormGroup: { BlockId: 0, SectorId: 0, BlockName: 0 },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //BlockMaster.renderBlockTable = BlockMaster.renderBlockTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillBlock = this.fillBlock.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneBlock/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["BlockId"] = response.data[0].BlockId;
                UserInput["SectorId"] = response.data[0].SectorId;
                UserInput["BlockName"] = response.data[0].BlockName;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillBlock();
            });
    }


    fillBlock() {
        axios.get('api/Masters/FillBlock')
            .then(response => {
                this.setState({ BlockList: response.data, loading: false });
            });
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });
        
    }

    handleSubmit(event) {
        event.preventDefault();

        const Block = this.state.FormGroup

        axios.post(`api/Masters/SaveBlock`, Block)
            .then(res => {
                this.FillForm(0);
                this.setState({
                    Message: res.data[0].Message
                });
            })
        $(document).ready(function () {
            $(".message").show();
        });
    }

    Delete(ID) {
        var body = document.getElementsByTagName('body')[0];
        var overlay = document.createElement('div');
        overlay.setAttribute("id", "myConfirm");
        var box = document.createElement('div');
        var boxchild = document.createElement('div');
        var p = document.createElement('p');
        p.appendChild(document.createTextNode("Are you sure"));
        boxchild.appendChild(p);
        var yesButton = document.createElement('button');
        var noButton = document.createElement('button');
        yesButton.appendChild(document.createTextNode('Yes'));
        yesButton.addEventListener('click', () => {

            axios.get(`api/Masters/DeleteBlock/${ID}`)
                .then(res => {
                    this.fillBlock();
                    this.setState({
                        Message: res.data[0].Message
                    });
                })

            $(document).ready(function () {
                $(".message").show();
            });

            body.removeChild(overlay);
        }, false);

        noButton.appendChild(document.createTextNode('No'));
        noButton.addEventListener('click', () => {

            body.removeChild(overlay);

        }, false);
        boxchild.appendChild(yesButton);
        boxchild.appendChild(noButton);
        box.appendChild(boxchild);
        overlay.appendChild(box)
        body.appendChild(overlay);
    }
    
 
    
    
    render() {

        const renderBlockTable = (BlockList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Sector Name</th>
                        <th>Block Name</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}>
                            <img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" />
                        </td>
                        <td>
                            <SectorDrop ProjectId="0" SectorId={this.state.FormGroup.SectorId} func={this.handleInputChange} />
                        </td>
                        <td>
                            <input type="text" name="BlockName" value={this.state.FormGroup.BlockName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {BlockList.map((forecast, i) =>

                        <tr key={forecast.BlockId}>
                            <td>{i + 1}</td>
                            <td>{forecast.SectorName}</td>
                            <td>{forecast.BlockName}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.BlockId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.BlockId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

        let BlockList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderBlockTable(this.state.BlockList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="Cityform" method="post" action="#">
                      <h1>Block</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">
                          {BlockList}
                                  
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
