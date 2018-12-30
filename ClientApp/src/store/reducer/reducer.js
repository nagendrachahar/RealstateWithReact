const INITIAL_STATE = {
  userName: 'Nagendra'
}

export default (states = INITIAL_STATE, action) => {
  switch (action.type) {

    case 'CHANGENAME':
      return ({
        ...states,
        userName: action.payload
      });

    default:
      return states;

  }
}
