import React from 'react';

const Message = (props) => (
   
    <ul className="message MessageType" style={{ position: "fixed", bottom: "10px", left: "10px", display: "none" }}>
        <li><strong>{props.message}</strong>&nbsp; &nbsp; &nbsp;</li>
        <li className="close-bt"></li>
    </ul>

);
export default Message;