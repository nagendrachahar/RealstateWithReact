export function changeState(updatedName) {

  return dispatch => {
    dispatch({ type: 'CHANGENAME', payload: updatedName });
  }

}
