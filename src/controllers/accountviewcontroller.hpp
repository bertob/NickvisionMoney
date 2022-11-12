#pragma once

#include <functional>
#include <locale>
#include <map>
#include <string>
#include "groupdialogcontroller.hpp"
#include "transactiondialogcontroller.hpp"
#include "../models/account.hpp"
#include "../models/group.hpp"
#include "../models/transaction.hpp"

namespace NickvisionMoney::Controllers
{
	/**
	 * A controller for the AccountView
	 */
	class AccountViewController
	{
	public:
		/**
		 * Constructs an AccountViewController
		 *
		 * @param path The path of the account to open
		 * @param locale The user's locale
		 * @param sendToastCallback A callback function for sending a toast notification to the UI
		 */
		AccountViewController(const std::string& path, const std::locale& locale, const std::function<void(const std::string& message)>& sendToastCallback);
		/**
		 * Gets the path of the account
		 *
		 * @returns The path of the account
		 */
		const std::string& getAccountPath() const;
		/**
		 * Gets the user's locale
		 *
		 * @returns The user's locale
		 */
		const std::locale& getLocale() const;
		/**
		 * Gets the total of the account as a string
		 *
		 * @returns The total of the account
		 */
		std::string getAccountTotalString() const;
		/**
		 * Gets the income of the account as a string
		 *
		 * @returns The income of the account
		 */
		std::string getAccountIncomeString() const;
		/**
		 * Gets the expense of the account as a string
		 *
		 * @returns The expense of the account
		 */
		std::string getAccountExpenseString() const;
		/**
		 * Gets the map of groups in the account
		 *
		 * @returns The groups in the account
		 */
		const std::map<unsigned int, NickvisionMoney::Models::Group>& getGroups() const;
		/**
		 * Gets the map of transactions in the account
		 *
		 * @returns The transaction in the account
		 */
		const std::map<unsigned int, NickvisionMoney::Models::Transaction>& getTransactions() const;
		/**
		 * Registers a callback for updating the UI when an account's info is changed
		 *
		 * @param callback A void() function
		 */
		void registerAccountInfoChangedCallback(const std::function<void()>& callback);
		/**
		 * Export the account as a CSV file
		 *
		 * @param path The path to the csv file
		 */
		void exportAsCSV(std::string& path);
		/**
		 * Import transactions from a CSV file
		 *
		 * @param path The path to the csv file
		 */
		void importFromCSV(std::string& path);
		/**
		 * Adds a new group to the account
		 *
		 * @param group The group to add
		 */
		void addGroup(const NickvisionMoney::Models::Group& group);
		/**
		 * Updates a group in the account
		 *
		 * @param group The group to update
		 */
		void updateGroup(const NickvisionMoney::Models::Group& group);
		/**
		 * Deletes a group in the account
		 *
		 * @param id The id of the group to delete
		 */
		void deleteGroup(unsigned int id);
		/**
		 * Creates a GroupDialogController for a new group
		 *
		 * @returns A new GroupDialogController
		 */
		GroupDialogController createGroupDialogController() const;
		/**
		 * Creates a GroupDialogController for an existing group
		 *
		 * @param id The id of the group to edit
		 * @returns A new GroupDialogController
		 */
		GroupDialogController createGroupDialogController(unsigned int id) const;
		/**
		 * Adds a new transaction to the account
		 *
		 * @param transaction The transaction to add
		 */
		void addTransaction(const NickvisionMoney::Models::Transaction& transaction);
		/**
		 * Updates a transaction in the account
		 *
		 * @param transaction The transaction to update
		 */
		void updateTransaction(const NickvisionMoney::Models::Transaction& transaction);
		/**
		 * Deletes a transaction in the account
		 *
		 * @param id The id of the transaction to delete
		 */
		void deleteTransaction(unsigned int id);
		/**
		 * Creates a TransactionDialogController for a new transaction
		 *
		 * @returns A new TransactionDialogController
		 */
		TransactionDialogController createTransactionDialogController() const;
		/**
		 * Creates a TransactionDialogController for an existing transaction
		 *
		 * @param id The id of the transaction to edit
		 * @returns A new TransactionDialogController
		 */
		TransactionDialogController createTransactionDialogController(unsigned int id) const;

	private:
		const std::locale& m_locale;
		NickvisionMoney::Models::Account m_account;
		std::function<void(const std::string& message)> m_sendToastCallback;
		std::function<void()> m_accountInfoChangedCallback;
	};
}